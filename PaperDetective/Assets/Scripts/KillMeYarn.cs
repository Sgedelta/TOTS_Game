using UnityEngine;
using Yarn.Unity;
public class KillMeYarn : MonoBehaviour
{
    [YarnCommand("killThis")]
    public void KillThis()
    {
        Destroy(this.gameObject);
    }
}
