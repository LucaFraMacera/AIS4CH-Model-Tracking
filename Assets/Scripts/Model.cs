
using System;

[Serializable]
public class Interaction {

    public bool rotateX;

    public bool rotateY;

    public bool rotateZ;

    public bool transformX;

    public bool transformY;

    public bool transformZ;

    override public string ToString(){
        return $"Rotation= X:{rotateX}, Y:{rotateY}, Z:{rotateZ}; Transform = X:{transformX}, Y:{transformY}, Z:{transformZ}";
    }

}


[Serializable]
public class Model{
    public string id;

    public string name;

    public float xOffset;

    public float yOffset;

    public float zOffset;

    public long fileSize;

    public string presignedUrl;

    public Interaction interaction;

    override public string ToString(){
        return $"ID: {this.id}, Name: {this.name}, XOffset: {this.xOffset}, YOffset: {this.yOffset}, ZOffset: {this.zOffset}, fileSize: {this.fileSize}, interaction: {this.interaction}";
    }
}

