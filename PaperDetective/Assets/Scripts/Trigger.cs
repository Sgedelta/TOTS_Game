using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class Trigger : MonoBehaviour
{
    public TriggerTemplate template;

    public void InteractWithItem(Item other)
    {
        if (template == other.Template)
        {
            //check if we should just switch this item to be the newly made item
            if(template.madeItem && template.deleteHeld)
            {
                other.InitAs(template.madeItem, 1);
            }
            //otherwise, do we need to make the item
            else if (template.madeItem && !template.deleteHeld)
            {
                other.CreateItem(template.madeItem, 1);
            }
            //otherwise, do we need to delete the item that is triggering this
            else if(template.deleteHeld)
            {
                other.amount = 0;
            }
            //ANY case, trigger dialog
            if (!SingletonComponent.Instances["Dialogue System Variant"].GetComponent<DialogueRunner>().IsDialogueRunning)
            {
                SingletonComponent.Instances["Dialogue System Variant"].GetComponent<DialogueRunner>().StartDialogue(template.successDialogNode);
            }

        }
        else
        {
            //we are in some sort of failure state
            if (!SingletonComponent.Instances["Dialogue System Variant"].GetComponent<DialogueRunner>().IsDialogueRunning)
            {
                string failureNode = template.failureDialogNode;
                //check if we have a specific. if we do, do that.
                if(template.failureItems.Contains(other.Template))
                {
                    failureNode = template.failureNodes[template.failureItems.IndexOf(other.Template)];
                }
                //if we don't, run the default one.
                SingletonComponent.Instances["Dialogue System Variant"].GetComponent<DialogueRunner>().StartDialogue(failureNode);

            }

        }

    }
}
