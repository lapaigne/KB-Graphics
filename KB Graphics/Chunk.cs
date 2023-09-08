using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KB_Graphics
{
    internal class Chunk
    {
        private long _x;
        private long _y;

        private const byte Width = 24;
        private const int WidthSqrd = 576;

        public long X { get { return _x; } }
        public long Y { get { return _y; } }

        private List<Tile> _tiles;


        public Chunk(long chunkX, long chunkY, ulong seed) {
            _tiles = new List<Tile>();
            for (int i = 0;i < WidthSqrd; i++) { 
                
            }
        }
    }
}
