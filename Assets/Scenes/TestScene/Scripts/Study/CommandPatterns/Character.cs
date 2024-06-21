namespace TestScene.Study
{
    using UnityEngine;
    public class Character : MonoBehaviour
    {
        private void Awake()
        {
            CommandManager.Instance.RegisterAllCommand(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                CommandManager.Instance.ExecuteCommand("Jump");
            else if(Input.GetKeyDown(KeyCode.A))
                CommandManager.Instance.ExecuteCommand("Attack");
        }

        [Command("Jump")]
        private void Jump()
        {
            Debug.Log("Jump!");
        }

        [Command("Attack")]
        private void Attack()
        {
            Debug.Log("Attack!");
        }
    }
}