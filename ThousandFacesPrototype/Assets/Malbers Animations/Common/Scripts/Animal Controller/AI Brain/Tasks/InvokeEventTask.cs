using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations.Controller.AI
{
    [CreateAssetMenu(menuName = "Malbers Animations/Pluggable AI/Tasks/Invoke Event")]
    public class InvokeEventTask : MTask
    {
        [Space]
        public UnityEvent Raise = new UnityEvent();
        
        public override void StartTask(MAnimalBrain brain, int index)
        {
            Raise.Invoke();
        }

        void Reset() { Description = "Raise the Event when the Task start. Use this only for Scriptable Assets"; }
    }
}
