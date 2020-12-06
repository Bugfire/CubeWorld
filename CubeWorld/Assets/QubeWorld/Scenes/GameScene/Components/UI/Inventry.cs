using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubeWorld.World.Objects;

namespace GameScene
{
    public class Inventry : MonoBehaviour
    {
        [SerializeField]
        private GameScene gameScene;
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private ItemButton templateItem;

        private List<ItemButton> items;
        private Dictionary<CWDefinition, Sprite> inventorySprites;

        #region Unity lifecycles

        void OnEnable()
        {
            CreateItems();
        }

        void OnDisable()
        {
            DestroyItems();
        }

        #endregion

        #region Public methods

        public void OnItemClicked(int index)
        {
            if (index >= 0 && index < gameManagerUnity.playerUnity.player.inventory.entries.Count)
            {
                gameManagerUnity.playerUnity.objectInHand = gameManagerUnity.playerUnity.player.inventory.entries[index].cwobject;
                gameScene.OnClose();
            }
        }

        #endregion

        #region Private methods

        private void DestroyItems()
        {
            if (items == null)
            {
                items = new List<ItemButton>(30);
                return;
            }
            for (var i = 0; i < items.Count; i++)
            {
                Destroy(items[i].gameObject);
            }
            items.Clear();
        }

        private void CreateTextures()
        {
            if (inventorySprites != null)
            {
                return;
            }

            inventorySprites = new Dictionary<CWDefinition, Sprite>();
            var tilesetTexture = (Texture2D)gameManagerUnity.material.mainTexture;

            foreach (var itemDefinition in gameManagerUnity.world.itemManager.itemDefinitions)
            {
                if (itemDefinition.type == CWDefinition.DefinitionType.Item)
                {
                    var materialName = "Items/" + itemDefinition.visualDefinition.material;
                    var texture = (Texture2D)Resources.Load(materialName, typeof(Texture2D));
                    inventorySprites[itemDefinition] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                }
            }

            foreach (var tileDefinition in gameManagerUnity.world.tileManager.tileDefinitions)
            {
                if (tileDefinition.tileType != CubeWorld.Tiles.TileDefinition.EMPTY_TILE_TYPE)
                {
                    Texture2D texture = null;

                    foreach (var material in tileDefinition.materials)
                    {
                        if (material >= 0)
                        {
                            // TODO: テクスチャのコピーをしているので大変無駄。
                            texture = GraphicsUnity.GetTilesetTexture(tilesetTexture, material);
                            break;
                        }
                    }
                    if (texture != null)
                    {
                        inventorySprites[tileDefinition] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    }
                }
            }
        }

        private void CreateItems()
        {
            CreateTextures();
            DestroyItems();

            var index = 0;
            foreach (var inventoryEntry in gameManagerUnity.playerUnity.player.inventory.entries)
            {
                var text = inventoryEntry.cwobject.definition.description;
                var texture = inventorySprites[inventoryEntry.cwobject.definition];
                var o = GameObject.Instantiate<ItemButton>(templateItem, Vector3.zero, Quaternion.identity, templateItem.transform.parent);
                o.gameObject.SetActive(true);
                o.Setup(this, index++, text, inventoryEntry.quantity, texture);
                items.Add(o);
            }
        }

        #endregion
    }
}
