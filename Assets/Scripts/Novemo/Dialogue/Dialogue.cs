using System;
using UnityEngine;

namespace Novemo.Dialogue
{
    [Serializable]
    public class Dialogue
    {
        public string name;
        
        [TextArea(3, 7)]
        public string[] sentences;
    }
}


