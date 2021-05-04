using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PersistentFactory : MonoBehaviour {
    List<GameObject> unusedItems = new List<GameObject>();
    List<GameObject> usedItems = new List<GameObject>();

    public GameObject itemPrefab;
    /*     public int preloadCount = 0;

        void Start() {
            for (int i = 0; i < preloadCount; i++) {
                GetOrCreateItem();
            }
            for (int i = 0; i < preloadCount; i++) {
                HideObject(unusedItems[0]);
            }
        } */
    private void Awake() {}
    public GameObject GetOrCreateItem() {
        GameObject newItem;
        if (unusedItems.Count == 0) {
            newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, transform);
            newItem.name = Time.time.GetHashCode().ToString();
        } else {
            newItem = unusedItems[0];
            unusedItems.Remove(newItem);
            newItem.SetActive(true);
        }
        usedItems.Add(newItem);
        return newItem;
    }
    public void HideObject(GameObject obj) {
        unusedItems.Add(obj);
        usedItems.Remove(obj);
        obj.transform.SetParent(transform);
        obj.SetActive(false);
    }
}