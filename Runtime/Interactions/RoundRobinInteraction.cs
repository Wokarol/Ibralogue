using UnityEngine;

namespace Ibralogue.Interactions
{
    /// <summary>
    /// Loops through each dialogue one by one and stops at the last one unless "Loop" is checked in which case it starts from the first dialogue.
    /// </summary>
    public class RoundRobinInteraction : BaseInteraction
    {
        private int _iteration;
        [SerializeField] private bool _loop;

        public override void StartDialogue()
        {
            base.StartDialogue();
            dialogueManager.StartConversation(InteractionDialogues[_iteration]);
            if (_iteration == InteractionDialogues.Length - 1)
            {
                _iteration = _loop ? 0 : _iteration;
                return;
            }
            _iteration++;
        }
    }
}
