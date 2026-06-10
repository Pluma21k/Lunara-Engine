using Lunara2D.Common;
using Lunara2D.Core.Components;

namespace Lunara2D.Core
{
    public class LEntity
    {
        private readonly List<LComponent> _components = new();

        public string Name { get; set; } = "Entity";

        public LEntity(string name)
        {
            Name = name;

            LSceneRegister.ActiveScene?.AddEntity(this);
            AddComponent<LTransformComponent>();
        }

        public T AddComponent<T>() where T : LComponent, new()
        {
            T component = new();

            component.Entity = this;

            _components.Add(component);

            component.Awake();

            return component;
        }

        public T GetComponent<T>() where T : LComponent
        {
            foreach (var component in _components)
            {
                if (component is T result)
                    return result;
            }

            return null;
        }

        public bool HasComponent<T>() where T : LComponent
        {
            return GetComponent<T>() != null;
        }

        public void Update(float deltaTime)
        {
            foreach (var component in _components)
                component.Update(deltaTime);
        }

        public void Render()
        {
            foreach (var component in _components)
                component.Render();
        }
    }
}