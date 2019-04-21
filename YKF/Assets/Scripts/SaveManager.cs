using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class serializableTransform
{
    public float x;
    public float y;
    public float z;
    public float xRot;
    public float yRot;
    public float zRot;
};
[Serializable]
public class SaveManager : MonoBehaviour
{
    public GameObject[] Units;

    void Update()
    {
        //save
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("saving!");
            Save();
        }
        //load
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("loading!");
            Load();
        }
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.data", FileMode.OpenOrCreate);
        Debug.Log("saved to: " + Application.persistentDataPath + "/playerInfo.data");

        ArrayList data = new ArrayList();
        foreach (GameObject g in Units) {
            serializableTransform t1 = new serializableTransform();
            t1.x = g.transform.position.x;
            t1.y = g.transform.position.y;
            t1.z = g.transform.position.z;
            t1.xRot = g.transform.eulerAngles.x;
            t1.yRot = g.transform.eulerAngles.y;
            t1.zRot = g.transform.eulerAngles.z;
            data.Add(t1);
        }
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.data")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(Application.persistentDataPath + "/playerInfo.data");
            ArrayList loadData = (ArrayList)(bf.Deserialize(file));

            for (int i = 0; i < Units.Length; i++) {
                serializableTransform transformData = (serializableTransform)loadData[i];
                Units[i].transform.position = new Vector3(transformData.x, transformData.y, transformData.z);
                Units[i].transform.eulerAngles = new Vector3(transformData.xRot, transformData.yRot, transformData.zRot);
            }
        }
    }
}
