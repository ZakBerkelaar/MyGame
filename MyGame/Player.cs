using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace MyGame
{
    public class Player : Entity
    {
        public ItemStack[] items = new ItemStack[10];
        public int CurrentItem { get; private set; }

        private float fallMultiplier = 2.5f;
        private float lowJumpMultiplier = 2f;

        private IControl jumpControl;
        private IControl leftControl;
        private IControl rightControl;

        private IControl slot0;
        private IControl slot1;
        private IControl slot2;
        private IControl slot3;
        private IControl slot4;
        private IControl slot5;
        private IControl slot6;
        private IControl slot7;
        private IControl slot8;
        private IControl slot9;

        public Player(): base(Entities.Player)
        {
            jumpControl = Input.CreateControl(new IDString("PlayerJump"), Key.W);
            leftControl = Input.CreateControl(new IDString("PlayerLeft"), Key.A);
            rightControl = Input.CreateControl(new IDString("PlayerRight"), Key.D);

            slot0 = Input.CreateControl(new IDString("PlayerSlot0"), Key.Number1);
            slot1 = Input.CreateControl(new IDString("PlayerSlot1"), Key.Number2);
            slot2 = Input.CreateControl(new IDString("PlayerSlot2"), Key.Number3);
            slot3 = Input.CreateControl(new IDString("PlayerSlot3"), Key.Number4);
            slot4 = Input.CreateControl(new IDString("PlayerSlot4"), Key.Number5);
            slot5 = Input.CreateControl(new IDString("PlayerSlot5"), Key.Number6);
            slot6 = Input.CreateControl(new IDString("PlayerSlot6"), Key.Number7);
            slot7 = Input.CreateControl(new IDString("PlayerSlot7"), Key.Number8);
            slot8 = Input.CreateControl(new IDString("PlayerSlot8"), Key.Number9);
            slot9 = Input.CreateControl(new IDString("PlayerSlot9"), Key.Number0);
        }

        protected override void Update()
        {
            if (velocity.x != 0)
            {
                //TODO: Not using Game.deltaTime
                velocity.x /= 2f;
            }

            if(velocity.y < 0)
            {
                velocity += Vector2.up * (-20f) * (fallMultiplier - 1) * world.deltaTime;
            }
            else if (velocity.y > 0 && !jumpControl.IsDown)
            {
                velocity += Vector2.up * (-20f) * (lowJumpMultiplier - 1) * world.deltaTime;
            }

            if (jumpControl.IsDownFrame)
                velocity.y = 25;

            if (leftControl.IsDown)
                velocity.x = -10;
            //if (Input.GetKey(Key.S))
            //    Console.WriteLine("Down");
            if (rightControl.IsDown)
                velocity.x = 10;

            if (slot1.IsDownFrame)
                CurrentItem = 1;
            else if (slot2.IsDownFrame)
                CurrentItem = 2;
            else if (slot3.IsDownFrame)
                CurrentItem = 3;
            else if (slot4.IsDownFrame)
                CurrentItem = 4;
            else if (slot5.IsDownFrame)
                CurrentItem = 5;
            else if (slot6.IsDownFrame)
                CurrentItem = 6;
            else if (slot7.IsDownFrame)
                CurrentItem = 7;
            else if (slot8.IsDownFrame)
                CurrentItem = 8;
            else if (slot9.IsDownFrame)
                CurrentItem = 9;
            else if (slot0.IsDownFrame)
                CurrentItem = 0;

            velocity.y -= 20 * world.deltaTime;
        }

        /// <summary>
        /// Gives the player an item
        /// </summary>
        /// <param name="item">The ItemStack to put in the player's inventory</param>
        /// <returns>Weather or not the ItemStack was successfully added</returns>
        public bool GiveItem(ItemStack item)
        {
            ItemStack loc = items.First(i => i == null);
            loc = item;
            return true; //TODO: Check if it actually worked
        }

        internal void MouseClick(MouseButtonEventArgs e)
        {
            if(e.Button == MouseButton.Left)
            {
                Vector2 offset = Game.activePlayer.position;
                //Vector2Int tilePos = new Vector2Int(Mathf.CeilToInt((e.X - (Width / 2f)) / 16f), Mathf.CeilToInt(((Height - e.Y) - (Height / 2f)) / 16f) + 1);
                Vector2Int tilePos = new Vector2Int(Mathf.CeilToInt(((e.X - (Game.window.Width / 2f)) / 16f) + offset.x), Mathf.CeilToInt((((Game.window.Height - e.Y) - (Game.window.Height / 2f)) / 16f) + 1 + offset.y));
                //tilePos += floored;
                Console.WriteLine(tilePos);
                Game.activeWorld.SetTile(tilePos, null);
                Game.networkerClient.SendMessage(new Networking.Packets.SetTilePacket(tilePos, null));
            }
            else if (e.Button == MouseButton.Right)
            {
                items[CurrentItem]?.item.UseItem(this, RenderHelper.ScreenToWorld(new Vector2(e.X, e.Y)), items[CurrentItem]);
            }
        }
    }
}
