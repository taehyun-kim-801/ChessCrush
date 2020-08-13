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
                return PeekDirector<T>();
            else
                return Use(typeName).GetComponent<T>();
        }

        public T PeekDirector<T>() where T : SubDirector => poolDictionary[typeof(T).Name][0].GetComponent<T>();
    }
}