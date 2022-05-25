using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.UI
{
    public class UIElement
    {
        public bool active;

        public List<UIElement> childern;

        public UIElement Parent { get; private set; }

        public delegate void MouseEvent(object e);

        public int top;
        public int left;
        public int width;
        public int height;

        public int globalTop => top + (Parent?.globalTop ?? 0);
        public int globalLeft => left + (Parent?.globalLeft ?? 0);

        public event MouseEvent OnMouseDown;
        public event MouseEvent OnMouseUp;
        public event MouseEvent OnMouseOver;
        public event MouseEvent OnMouseOut;

        public UIElement()
        {
            active = true;
            childern = new List<UIElement>();
        }

        public void AddChild(UIElement element)
        {
            if (element.Parent != null)
                element.Parent.RemoveChild(element);

            element.Parent = this;
            childern.Add(element);
        }

        public void RemoveChild(UIElement element)
        {
            childern.Remove(element);
            element.Parent = null;
        }

        public virtual void Draw()
        {
            DrawSelf();
            foreach (var item in childern)
            {
                item.Draw();
            }
        }

        protected virtual void DrawSelf()
        {

        }

        public virtual void MouseDown()
        {
            OnMouseDown?.Invoke(null);
        }

        public virtual void MouseUp()
        {
            OnMouseUp?.Invoke(null);
        }

        public virtual void MouseOver()
        {
            OnMouseOver?.Invoke(null);
        }

        public virtual void MouseOut()
        {
            OnMouseOut?.Invoke(null);
        }
    }
}
