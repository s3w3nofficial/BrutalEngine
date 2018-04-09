using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Input;

namespace BrutalEngine.input
{
    class KeybordInput
    {
        private List<Key> _keyDown = new List<Key>();

        public KeybordInput(GameWindow window)
        {
            window.KeyDown += (o, e) =>
            {
                if (!_keyDown.Contains(e.Key))
                    _keyDown.Add(e.Key);
            };

            window.KeyUp += (o, e) => { _keyDown.Remove(e.Key); };
        }

        public bool IsKeyDown(Key key)
        {
            return _keyDown.Contains(key);
        }
    }
}
