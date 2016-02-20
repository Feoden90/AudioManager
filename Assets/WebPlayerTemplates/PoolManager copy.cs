using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CustomManager
{
	//Base pool manager class. Will be specialized by children.
	public class PoolManager2 : MonoBehaviour
	{

		//this is only for preinstantiating a set number of objects;
		public int startingNumber;

		//lists that will keep track of object usage;
		private List<string> freeObjects;
		private List<string> busyObjects;

		//the object container;
		protected Dictionary<string,GameObject> myobjectList;

		protected virtual void Start ()
		{
			//initialization of containers;
			freeObjects = new List<string> ();
			busyObjects = new List<string> ();
			myobjectList = new Dictionary<string, GameObject> ();

			//instantiation of the starting objects. their number will be extended at runtime;
			for (int i = 0; i < startingNumber; i++) {
				InstantiateNewObject ();
			}
		}

		//method that creates the object instance. empty for father class.
		protected virtual GameObject CreateObjectProperties ()
		{
			return new GameObject ();
		}

		//method that creates and makes usable the new object.
		private void InstantiateNewObject ()
		{
			//Setup the game object as needed;
			GameObject tmp = CreateObjectProperties ();
			//add the game object as child of the manager;
			tmp.transform.SetParent (this.transform);
			//create an ID for the object, it will be used as access key;
			string key = tmp.GetInstanceID ().ToString ();
			freeObjects.Add (key);
			myobjectList.Add (key, tmp);
		}

		//method that fetches the first free object (creates a new one if none).
		protected GameObject GetNextFreeObjectInstance ()
		{
			//if no free objects, create a new one;
			if (freeObjects.Count == 0) {
				InstantiateNewObject ();
			}
			//get the first free object and flag it as occupied;
			string key = freeObjects [0];
			freeObjects.Remove (key);
			busyObjects.Add (key);
			//return the object instance;
			return myobjectList [key];
		}

		//method that frees an object instance so that it can be reused.
		public virtual void ReleaseObjectInstance (string key)
		{
			//object gets flagged as free again;
			freeObjects.Add (key);
			busyObjects.Remove (key);
		}

	}
}