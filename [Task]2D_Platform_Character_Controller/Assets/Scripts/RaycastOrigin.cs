
    using UnityEngine;

    public struct Detector {
        public Vector2 start;
        public Vector2 end;
        public Vector2 dir;
        public Detector(Vector2 start, Vector2 end, Vector2 dir)
        {
            this.start = start;
            this.end = end;
            this.dir = dir;
        }


    }