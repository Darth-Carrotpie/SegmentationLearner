using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PointCloudCoordinator : Singleton<PointCloudCoordinator> {

    public GameObject pointPrefab;

    int currentPoint;
    List<Transform> points = new List<Transform>();

    public static void RegeneratePointCloud(int amount) {
        Instance.ClearPoints();
        Instance.GeneratePoints(amount);
    }

    void ClearPoints() {
        for (int i = 0; i < points.Count; i++) {
            Destroy(points[i].gameObject);
        }
        points.Clear();
    }

    void GeneratePoints(int amount) {
        for (int i = 0; i < amount; i++) {
            Vector3 pos = GetRandomLocation() + RndHeight();
            Vector3 target = GetRandomLocation() + RndHeight() - new Vector3(0, -1f, 0); //We want targets to be slightly lower, since there are more items on the floor than on the ceiling...
            Quaternion rot = Quaternion.LookRotation(pos - target, Vector3.up * 20f);
            GameObject newPoint = Instantiate(pointPrefab, Vector3.zero, rot, transform);
            newPoint.transform.position = pos;
            points.Add(newPoint.transform);
        }
    }

    public static Transform NextPoint() {
        Instance.currentPoint++;
        if (Instance.currentPoint == Instance.points.Count) {
            Instance.currentPoint = 0;
        }
        return Instance.points[Instance.currentPoint];
    }

    public static void ResetIndex() {
        Instance.currentPoint = 0;
    }

    public static void ShowPoints(bool toState = true) {
        foreach (Transform p in Instance.points) { p.gameObject.SetActive(toState); }
    }

    public Vector3 RndHeight() {
        return new Vector3(0, Random.Range(-ConstantsBucket.HeightDelta, ConstantsBucket.HeightDelta) + ConstantsBucket.HeightBias, 0);
    }

    public Vector3 GetRandomLocation() {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        int t = Random.Range(0, navMeshData.indices.Length - 3);
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);
        return point;
    }
}