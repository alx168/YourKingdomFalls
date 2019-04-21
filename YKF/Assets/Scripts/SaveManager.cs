using UnityEngine;
using UnityEngine.UI;
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
    public Text t;

    private string dataLocation;
    private void Start()
    {
        dataLocation = Application.persistentDataPath + "/playerInfo.data";
    }
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
        FileStream file = File.Create(dataLocation);
        Debug.Log("saved to: " + dataLocation);

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

        t.text = "Saved to: " + dataLocation;
        StartCoroutine(waitThenClear(t));
    }

    public void Load()
    {
        if (File.Exists(dataLocation)){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(dataLocation);
            ArrayList loadData = (ArrayList)(bf.Deserialize(file));
            Debug.Log("Loaded from: " + dataLocation);

            for (int i = 0; i < Units.Length; i++) {
                serializableTransform transformData = (serializableTransform)loadData[i];
                Units[i].transform.position = new Vector3(transformData.x, transformData.y, transformData.z);
                Units[i].transform.eulerAngles = new Vector3(transformData.xRot, transformData.yRot, transformData.zRot);
            }

            t.text = "Load from: " + dataLocation;
            StartCoroutine(waitThenClear(t));
        }
    }
    IEnumerator waitThenClear(Text t) {
        yield return new WaitForSeconds(3);
        t.text = "";
    }
}
