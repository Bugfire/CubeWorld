﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CubeWorld.Tiles.Rules;
using CubeWorld.Tiles;
using CubeWorld.World.Objects;
using CubeWorld.Items;

namespace GameScene
{
    public class PlayerControllerUnity : MonoBehaviour
    {
        public float sensitivityX = 15.0f;
        public float sensitivityY = 15.0f;

        public float minimumX = -360.0f;
        public float maximumX = 360.0f;

        public float minimumY = -89.0f;
        public float maximumY = 89.0f;

        private float rotationYaxis = 0.0f;
        private float rotationXaxis = 0.0f;

        private Quaternion originalCameraRotation;
        private Quaternion originalPlayerRotation;

        public PlayerUnity playerUnity;

        private GameObject hand;

        private GameObject goInHand;
        private CWObject currentObjectInHand;

        private Vector3 positionHand_Tile = new Vector3(0.15f, -0.15f, 0.3f);
        private Vector3 scaleHand_Tile = new Vector3(0.1f, 0.1f, 0.1f);
        private Quaternion rotationHand_Tile = Quaternion.Euler(-15.0f, 0.0f, 15.0f);

        private Vector3 positionHand_Item = new Vector3(0.15f, -0.15f, 0.2f);
        private Vector3 scaleHand_Item = new Vector3(0.1f, 0.1f, 0.1f);
        private Quaternion rotationHand_Item = Quaternion.Euler(0.0f, 55.0f, 0.0f);

        private Vector3 positionHand_Current;
        private Vector3 scaleHand_Current;
        private Quaternion rotationHand_Current;

        private bool firstUpdate = true;

        private GameController gameController
        {
            get
            {
                return playerUnity.gameManagerUnity.gameController;
            }
        }

        private GameScene gameScene
        {
            get
            {
                return playerUnity.gameScene;
            }
        }

        public void Start()
        {
            originalCameraRotation = playerUnity.mainCamera.transform.localRotation;
            originalPlayerRotation = transform.localRotation;

            hand = new GameObject();
            hand.name = "Hand";

            hand.transform.parent = playerUnity.mainCamera.transform;

            hand.transform.localPosition = positionHand_Tile;
            hand.transform.localScale = scaleHand_Tile;
            hand.transform.localRotation = rotationHand_Tile;
        }

        public void UpdateControlled()
        {
            if (gameScene.IsPlayable)
            {
                if (firstUpdate)
                {
                    rotationYaxis = playerUnity.player.rotation.y;
                    rotationXaxis = playerUnity.player.rotation.x;
                    firstUpdate = false;
                }

                if (Input.GetKeyDown(KeyCode.R))
                    playerUnity.player.ResetPosition();

                if (Input.GetKeyDown(KeyCode.C))
                    playerUnity.ChangeCamera();

                UpdateJump();
                UpdateMovement();
                UpdateCameraRotation();
                UpdateUserActions();
                UpdateItemOnHand();

                playerUnity.player.rotation.y = rotationYaxis;
                playerUnity.player.rotation.x = rotationXaxis;
            }
            else
            {
                playerUnity.player.input.jump = false;
                playerUnity.player.input.moveDirection = new CubeWorld.Utils.Vector3(0, 0, 0);
            }
        }

        private void ExecuteHandUseAnimation()
        {
            handUseAnimationTimer = 0.5f;
        }

        private float handUseAnimationTimer;
        private float handMovementTimer;

        private void UpdateItemOnHand()
        {
            if (currentObjectInHand != playerUnity.objectInHand)
            {
                if (goInHand)
                {
                    playerUnity.gameManagerUnity.objectsManagerUnity.RemoveGameObject(goInHand);

                    goInHand = null;
                }

                this.currentObjectInHand = playerUnity.objectInHand;

                if (currentObjectInHand != null)
                {
                    goInHand = playerUnity.gameManagerUnity.objectsManagerUnity.CreateGameObjectFromObject(currentObjectInHand);

                    goInHand.transform.parent = hand.transform;
                    goInHand.transform.localScale = new Vector3(1, 1, 1);
                    goInHand.transform.localPosition = new Vector3(0, 0, 0);
                    goInHand.transform.localRotation = Quaternion.identity;

                    switch (currentObjectInHand.definition.type)
                    {
                        case CWDefinition.DefinitionType.Item:
                            positionHand_Current = positionHand_Item;
                            scaleHand_Current = scaleHand_Item;
                            rotationHand_Current = rotationHand_Item;
                            break;

                        case CWDefinition.DefinitionType.Tile:
                            positionHand_Current = positionHand_Tile;
                            scaleHand_Current = scaleHand_Tile;
                            rotationHand_Current = rotationHand_Tile;
                            break;
                    }

                    hand.transform.localPosition = positionHand_Current;
                    hand.transform.localScale = scaleHand_Current;
                    hand.transform.localRotation = rotationHand_Current;
                }
            }

            if (handUseAnimationTimer <= 0.0f)
            {
                if (playerUnity.player.input.moveDirection.magnitude > 0.0f)
                {
                    handMovementTimer += Time.deltaTime;

                    float deltaY = Mathf.Sin(handMovementTimer * 10) * 0.02f;
                    float deltaX = Mathf.Sin(handMovementTimer * 10) * 0.01f;

                    hand.transform.localPosition = positionHand_Current + new Vector3(deltaX, deltaY, 0.0f);
                }
                else
                {
                    handMovementTimer = 0.0f;
                    hand.transform.localPosition = positionHand_Current;
                }
            }
            else
            {
                if (currentObjectInHand != null)
                {
                    float deltaRotation = Mathf.Sin(handUseAnimationTimer * 2.0f * Mathf.PI) * 30;

                    hand.transform.localPosition = positionHand_Current;

                    switch (currentObjectInHand.definition.type)
                    {
                        case CWDefinition.DefinitionType.Tile:
                            hand.transform.localRotation = rotationHand_Current * Quaternion.Euler(deltaRotation, 0, 0);
                            break;

                        case CWDefinition.DefinitionType.Item:
                            hand.transform.localRotation = rotationHand_Current * Quaternion.Euler(0, 0, deltaRotation);
                            break;
                    }
                }

                handUseAnimationTimer -= Time.deltaTime;

                if (handUseAnimationTimer <= 0.0f)
                {
                    hand.transform.localRotation = rotationHand_Current;
                    handUseAnimationTimer = 0.0f;
                }
            }
        }

        private float userActionCooldown;

        private void UpdateUserActions()
        {
            if (userActionCooldown > 0.0f)
            {
                userActionCooldown -= Time.deltaTime;
            }
            if (userActionCooldown > 0.0f)
            {
                return;
            }

            if (!gameScene.IsPlayable)
            {
                return;
            }
            if (!gameController.Action)
            {
                return;
            }

            var actionPos = gameController.ActionPos;
            var screenSpacePos = new Vector3(actionPos.x, actionPos.y, playerUnity.mainCamera.nearClipPlane);
            var screenWorldPos = playerUnity.mainCamera.ScreenToWorldPoint(screenSpacePos);
            var cameraPos = playerUnity.mainCamera.transform.position;
            var cameraFwdRaw = screenWorldPos - cameraPos;
            if (cameraFwdRaw.sqrMagnitude <= .0000001f)
            {
                return;
            }
            var cameraFwd = cameraFwdRaw.normalized;

            var raycastResult = CubeWorld.Utils.Graphics.RaycastTile(
                playerUnity.player.world,
                GraphicsUnity.Vector3ToCubeWorldVector3(cameraPos),
                GraphicsUnity.Vector3ToCubeWorldVector3(cameraFwd),
                10.0f, true, false);

            ExecuteHandUseAnimation();
            userActionCooldown = 0.2f;
            if (!raycastResult.hit)
            {
                return;
            }

            if (gameController.ActionIsAttack)
            {
                executeAttack(raycastResult.position);
            }
            else
            {
                executeBuild(raycastResult.position, raycastResult.face);
            }
        }

        private void executeAttack(TilePosition tilePosition)
        {
            if (tilePosition.x <= 0 || tilePosition.x >= playerUnity.player.world.tileManager.sizeX - 1 &&
                tilePosition.z <= 0 || tilePosition.z >= playerUnity.player.world.tileManager.sizeZ - 1 &&
                tilePosition.y <= 0)
            {
                return;
            }
            if (playerUnity.player.world.tileManager.HasTileActions(tilePosition, TileActionRule.ActionType.CLICKED))
            {
                playerUnity.player.world.gameplay.TileClicked(tilePosition);
                return;
            }
            if (playerUnity.objectInHand == null)
            {
                return;
            }
            switch (playerUnity.objectInHand.definition.type)
            {
                case CWDefinition.DefinitionType.Item:
                    playerUnity.gameManagerUnity.fxManagerUnity.PlaySound("hitmetal", playerUnity.player.position);
                    playerUnity.player.world.gameplay.TileHit(tilePosition, ((Item)playerUnity.objectInHand).itemDefinition);
                    break;

                default:
                    playerUnity.gameManagerUnity.fxManagerUnity.PlaySound("hit", playerUnity.player.position);
                    playerUnity.player.world.tileManager.DamageTile(tilePosition, 1);
                    break;
            }
        }

        private void executeBuild(TilePosition tilePosition, CubeWorld.Utils.Graphics.Faces face)
        {
            if (playerUnity.objectInHand == null || playerUnity.objectInHand.definition.type != CWDefinition.DefinitionType.Tile)
            {
                return;
            }
            TileDefinition tileDefinition = (TileDefinition)playerUnity.objectInHand.definition;
            TilePosition tileCreatePosition = tilePosition + CubeWorld.Utils.Graphics.GetFaceNormal(face);

            //Don't create tile on top of the world, because no triangles are drawn on the border!
            if (tileCreatePosition.y >= playerUnity.player.world.tileManager.sizeY - 1 ||
                !playerUnity.player.world.tileManager.IsValidTile(tileCreatePosition) ||
                playerUnity.player.world.tileManager.GetTileSolid(tileCreatePosition))
            {
                return;
            }
            if (playerUnity.player.world.avatarManager.IsTileBlockedByAnyAvatar(tileCreatePosition))
            {
                return;
            }
            playerUnity.player.world.gameplay.CreateTile(tileCreatePosition, tileDefinition.tileType);

            playerUnity.player.inventory.RemoveFromDefinition(tileDefinition, 1);

            if (playerUnity.player.inventory.HasMoreOfDefinition(tileDefinition) == false)
                playerUnity.objectInHand = null;
        }

        private void UpdateJump()
        {
            playerUnity.player.input.jump = gameController.Jump;
        }

        private void UpdateMovement()
        {
            var move = gameController.Move;
            var dirWalk = transform.forward * move.y;
            var dirStrafe = transform.right * move.x;
            var dir = dirWalk + dirStrafe;
            dir.y = 0;

            playerUnity.player.input.moveDirection = GraphicsUnity.Vector3ToCubeWorldVector3(dir);
        }

        private void UpdateCameraRotation()
        {
            var rawRotation = gameController.Rotation;
            rotationYaxis += rawRotation.x * sensitivityX;
            rotationXaxis += rawRotation.y * sensitivityY;

            rotationYaxis = ClampAngle(rotationYaxis, minimumX, maximumX);
            rotationXaxis = ClampAngle(rotationXaxis, minimumY, maximumY);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationYaxis, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationXaxis, Vector3.left);

            playerUnity.mainCamera.transform.localRotation = originalCameraRotation * yQuaternion;
            transform.localRotation = originalPlayerRotation * xQuaternion;
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

    }
}
