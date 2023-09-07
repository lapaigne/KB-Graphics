using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KB_Graphics
{
    internal class Tile
    {
        private long _x;
        private long _y;

        private byte _type;
        private bool _isWalkable;

        public long X
        {
            get { return _x; }
        }
        public long Y
        {
            get { return _y; }
        }
        public byte Type
        {
            get { return _type; }
        }
        private enum TileTypes
        {
            Hard
        }
    }
}
