﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public Text nameText;
        public Text dialogueText;

        public Animator animator;
    
        private Queue<string> sentences;

        // Start is called before the first frame update
        void Start()
        {
            sentences = new Queue<string>();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                DisplayNextSentence();
            }
        }

        public void StartDialogue(Dialogue dialogue)
        {
            animator.SetBool("IsOpen", true);
        
            nameText.text = dialogue.name;
        
            sentences.Clear();

            foreach (var sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
            dialogueText.text = sentence;
        }

        void EndDialogue()
        {
            animator.SetBool("IsOpen", false);
        }
    }
}
