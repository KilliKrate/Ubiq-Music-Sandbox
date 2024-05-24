using MessageTypes;
using Ubiq.Messaging;
using UnityEngine;

public class SyncedTransform : MonoBehaviour
{
    public NetworkContext context;
    private Vector3 lastPosition;

    public void Start()
    {
        context = NetworkScene.Register(this);
    }

    void Update()
    {
        if (lastPosition != transform.position)
        {
            lastPosition = transform.position;

            context.SendJson(new Position()
            {
                position = transform.position,
                rotation = transform.rotation,
            });
        }
    }
    // TODO: Test if different components talk on different contexts altogether, thus removing the need for a messageType check
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        Message m = message.FromJson<Message>();
        if (m.messageType == MessageType.Position)
        {
            Position content = message.FromJson<Position>();
            transform.position = content.position;
            transform.rotation = content.rotation;
            lastPosition = transform.position;
        }
            
    }

    public void SyncLastPosition()
    {
        lastPosition = transform.position;
    }
}
