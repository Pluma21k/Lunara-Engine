namespace Lunara2D.Core
{
    public abstract class LComponent
    {
        public LEntity? Entity { get; internal set; }

        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update(float deltaTime) { }
        public virtual void Render() { }
        public virtual void Destroy() { }
    }
}