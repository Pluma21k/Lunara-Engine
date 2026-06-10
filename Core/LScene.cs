using Lunara2D.Common;

namespace Lunara2D.Core
{
    public class LScene
    {
        private List<LEntity> entities = new();

        internal void AddEntity(LEntity entity)
        {
            if (!entities.Contains(entity))
            {
                LDebug.LogInfo("Created new entity name: " + entity.Name);
                entities.Add(entity);
            }
        }

        internal void RemoveEntity(LEntity entity)
        {
            if (entities.Contains(entity))
            {
                entities.Remove(entity);
                LDebug.LogInfo("Removed the entity name: " + entity.Name);
            }
        }


        public List<LEntity> GetRootEntities()
        {
            return entities;
        }

        internal void Dispose()
        {
            entities.Clear();
        }

        internal void Update()
        {
            foreach (var e in entities)
            {
                e.Update(LTime.deltaTime);
            }
        }

        internal void Render()
        {
            foreach (var e in entities)
            {
                e.Render();
            }
        }
    }
}
