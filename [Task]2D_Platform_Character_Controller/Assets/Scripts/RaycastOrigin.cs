
    using System;
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
    [Serializable]
    public struct CollisionInfo {
        public bool isTop;
        public bool isBottom;
        public bool isLeft;
        public bool isRight;

        public void CheckTopBottom(bool bTop,bool bBottom)
        {
            isTop = bTop;
            isBottom = bBottom;
        }

        public void CheckSide(bool bLeft, bool bRight)
        {
            isLeft = bLeft;
            isRight = bRight;
        }
        public void Init()
        {
            isTop = false;
            isBottom = false;
            isLeft = false;
            isRight = false;
        }
    }