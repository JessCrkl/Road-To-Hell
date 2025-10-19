using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    
    protected void FinishedQuestStep()
    {
        if(!isFinished)
        {
            isFinished = true;

            // TO DO: advance quest aka story forward now that quest is done

            Destroy(this.gameObject);
        }
    }
}
