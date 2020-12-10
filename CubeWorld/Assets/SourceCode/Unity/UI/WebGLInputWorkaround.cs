using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// WebGL のマルチタッチが壊れているため、それに対応する悲しみのタッチ入力キャンセラー
//   マルチタッチ時に低レベル TouchPhase.Ended が飛んでこないことが稀によくあるので、
//     onPressed の後の onReleased, onTapped
//     onBeginDrag の後の onEndDrag
//   がこない可能性がある
//
// 有効化方法:
//   StandaloneInputModule と同じ GameObject に設置をする
//
// 実装:
//   幸い StandaloneInputModule には Input をオーバライドする機能があるので、それを利用する
//
// 修正1:
//   そこで、5フレーム呼ばれてない fingerId を持つ Touch が存在した場合 Ended を追加する
//   (本来は Ended, Canceled, Moved, Stationary のどれかが飛んでくる)
//
// 修正2:
//   ついでに、全く同じ Began イベントが複数フレームに渡って飛んできた場合にそれを削除する
//   (本来は Stationary であろうが)
//
// しかし、Unity Native 実装層ではあいかわらずメモリリークしていると思われるので、悲しみは止まらない

public class WebGLInputWorkaround : BaseInput
{
    private struct TouchInfo
    {
        public bool used;
        public int frameCount;
        public Touch touch;
        public TouchInfo(int curFrameCount, ref Touch touchSrc)
        {
            used = true;
            frameCount = curFrameCount;
            touch = touchSrc;
        }
        public bool isStalled(int curFrameCount)
        {
            return used && frameCount < curFrameCount - 5;
        }
        public TouchInfo createEnded()
        {
            var newTouch = new Touch()
            {
                phase = TouchPhase.Ended,
                fingerId = touch.fingerId,
                position = touch.position,
                rawPosition = touch.rawPosition,
                type = touch.type,
                deltaTime = 0.1f, // 適当
                // 他のパラメータは0でよい。
            };
            return new TouchInfo(0, ref newTouch);
        }
        public bool Compare(ref Touch other)
        {
            // Equals(), == を Override するのが普通だが、ここでしか使わないので普通の関数にする。
            return (
                touch.altitudeAngle == other.altitudeAngle &&
                touch.azimuthAngle == other.azimuthAngle &&
                touch.deltaPosition == other.deltaPosition &&
                touch.deltaTime == other.deltaTime &&
                touch.fingerId == other.fingerId &&
                touch.maximumPossiblePressure == other.maximumPossiblePressure &&
                touch.phase == other.phase &&
                touch.position == other.position &&
                touch.pressure == other.pressure &&
                touch.radius == other.radius &&
                touch.radiusVariance == other.radiusVariance &&
                touch.rawPosition == other.rawPosition &&
                touch.tapCount == other.tapCount &&
                touch.type == other.type
            );
        }
    };

    private const string VERSION = "1.0.0";

    private bool workaroundEnabled = false;
    private int lastCheckedFrameCount = -1;

    // あまり動的にメモリを確保したくないのと、実装を楽にしたい中間解として List<struct> になった。
    private List<TouchInfo> activeTouches = new List<TouchInfo>(100);
    private List<TouchInfo> patchedTouches = new List<TouchInfo>(100);

    #region Unity lifecycles

    protected override void Start()
    {
        base.Start();
        Debug.LogFormat("Regsiter WebGLInputWorkaround {0}", VERSION);
        gameObject.GetComponent<StandaloneInputModule>().inputOverride = this;
    }

    #endregion

    #region BaseInput Overrides

    public override int touchCount
    {
        get
        {
            check();
            if (workaroundEnabled)
            {
                return patchedTouches.Count;
            }
            else
            {
                return Input.touchCount;
            }
        }
    }

    public override Touch GetTouch(int index)
    {
        check();
        if (workaroundEnabled)
        {
            return patchedTouches[index].touch;
        }
        else
        {
            return Input.GetTouch(index);
        }
    }

    #endregion

    #region Private methods

    private void check()
    {
        if (Input.touchSupported && workaroundEnabled == false)
        {
            Debug.LogFormat("Enabling WebGLInputWorkaround {0}", VERSION);
            workaroundEnabled = true;
        }
        if (!workaroundEnabled)
        {
            return;
        }

        var frameCount = Time.frameCount;
        if (frameCount == lastCheckedFrameCount)
        {
            return;
        }
        lastCheckedFrameCount = frameCount;
        patchedTouches.Clear();
        updateActiveAndCreatePatched(frameCount);
        removeStalledAndCreateEndedPatched(frameCount);
    }

    private void updateActiveAndCreatePatched(int frameCount)
    {
        // このフレームで動きのあったタッチの一覧を取得
        for (var i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            var fingerId = touch.fingerId;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (!registerActiveTouch(ref touch, frameCount))
                    {
                        // (修正2) Began の後で同じ Began が飛んできた。無視する
                        // Debug.LogFormat("*** IGNORE Began: fingerId:{0} position:{1}", activeTouches[i].touch.fingerId, activeTouches[i].touch.position);
                        continue;
                    }
                    break;
                case TouchPhase.Moved:
                    registerActiveTouch(ref touch, frameCount);
                    break;
                case TouchPhase.Stationary:
                    registerActiveTouch(ref touch, frameCount);
                    break;
                case TouchPhase.Ended:
                    unregisterActiveTouch(fingerId);
                    break;
                case TouchPhase.Canceled:
                    unregisterActiveTouch(fingerId);
                    break;
            }
            // Debug.LogFormat("{0}: fingerId:{1} position:{2}", touch.phase, touch.fingerId, touch.position);
            patchedTouches.Add(new TouchInfo(frameCount, ref touch));
        }
    }

    private void removeStalledAndCreateEndedPatched(int frameCount)
    {
        for (var i = 0; i < activeTouches.Count; i++)
        {
            if (activeTouches[i].isStalled(frameCount))
            {
                // (修正1) 放置されてから 5 フレーム以降のタッチの痕跡に対して開放を試みる。
                // Debug.LogFormat("*** ADD Ended: fingerId:{0} position:{1}", activeTouches[i].touch.fingerId, activeTouches[i].touch.position);
                patchedTouches.Add(activeTouches[i].createEnded());
                activeTouches[i] = new TouchInfo();
            }
        }
    }

    private bool registerActiveTouch(ref Touch touch, int frameCount)
    {
        int firstUnused = -1;
        var newTouchInfo = new TouchInfo(frameCount, ref touch);
        for (var i = 0; i < activeTouches.Count; i++)
        {
            if (activeTouches[i].used)
            {
                if (activeTouches[i].touch.fingerId == touch.fingerId)
                {
                    var modified = !activeTouches[i].Compare(ref touch);
                    activeTouches[i] = newTouchInfo;
                    return modified;
                }
            }
            else if (firstUnused < 0)
            {
                firstUnused = i;
            }
        }

        if (firstUnused >= 0)
        {
            activeTouches[firstUnused] = newTouchInfo;
        }
        else
        {
            activeTouches.Add(newTouchInfo);
        }
        return true;
    }

    private void unregisterActiveTouch(int fingerId)
    {
        for (var i = 0; i < activeTouches.Count; i++)
        {
            if (activeTouches[i].used && activeTouches[i].touch.fingerId == fingerId)
            {
                activeTouches[i] = new TouchInfo();
            }
        }
    }

    #endregion
}
