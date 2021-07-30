using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Ibralogue
{
   public static class DialogueParser
   {
      /// <remarks>
      /// The Tokens are a representation of the attribute of the current line we are parsing
      /// which provides additional information about the lexeme that the token represents. 
      /// </remarks>
      public enum Tokens
      {
         ILLEGAL,
         SPEAKER,
         SENTENCE,
         IMAGE,
      }
      
      
      /// <summary>
      /// The GetLineToken function checks what the line it is given starts with and associates a "token" with it
      /// according to that.
      /// </summary>
      private static Tokens GetLineToken(string line)
      {
         if (line.StartsWith("~")) return Tokens.SPEAKER;
         if (line.StartsWith("-")) return Tokens.SENTENCE;
         if (line.StartsWith("!")) return Tokens.IMAGE;
         return Tokens.ILLEGAL;
      }   
      
      /// <summary>
      /// The ParseDialogue function returns an array of dialogues and associates information
      /// with each element in the array. Speaker Name, Sentence, Image etc.
      /// </summary>
      public static List<Dialogue> ParseDialogue(TextAsset dialogueAsset)
      {
         string dialogueText = dialogueAsset.text;
         string[] fLines = dialogueText.Split('\n');
         
         List<Dialogue> dialogues = new List<Dialogue>();
         Dialogue dialogue = new Dialogue{sentences = new List<string>()};

         for (int index = 0; index < fLines.Length; index++)
         {
            string line = fLines[index];
            Tokens token = GetLineToken(line);
            
            string processedLine = Regex.Replace(line, "^(~|-|!)", string.Empty);
            
            switch (token)
            {
               case Tokens.SPEAKER when dialogue.speaker == null:
                  dialogue.speaker = processedLine;
                  break;
               case Tokens.SPEAKER:
                  dialogues.Add(dialogue);
                  dialogue = new Dialogue{sentences = new List<string>()};
                  dialogue.speaker = processedLine;
                  break;
               case Tokens.SENTENCE:
                  dialogue.sentences.Add(processedLine);
                  break;
               case Tokens.IMAGE:
                  string imagePath = Regex.Replace(processedLine.Replace("\"", ""), @"\s+", "");
                  if(Resources.Load(imagePath) == null) throw new Exception($"[Ibralogue] Invalid image path {processedLine} at {index+1}");
                  dialogue.speakerImage = Resources.Load<Sprite>(imagePath);
                  break;
               case Tokens.ILLEGAL:
                  throw new Exception($"[Ibralogue] Illegal Starter Token at Line {index+1} in {dialogueAsset.name}");
               default:
                  throw new ArgumentOutOfRangeException($"[Ibralogue] Unexpected Argument Received at {line+1}");
            }
            if (index == fLines.Length - 1) dialogues.Add(dialogue); 
         }
         return dialogues;
      }
   }
}