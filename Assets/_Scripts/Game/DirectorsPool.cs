namespace ChessCrush.Game
{
    public class DirectorsPool: ObjectPool
    {
        private void Awake()
        {
            base.Awake();
        }

        public T UseDirector<T>() where T: SubDirector
        {
            var typeName = typeof(T).Name;

            if (poolDictionary[typeName].Exists(obj => obj.activeSelf))
                return poolDictionary[typeof(T).Name][0].GetComponent<T>();
            else
                return Use(typeName).GetComponent<T>();
        }
    }
}