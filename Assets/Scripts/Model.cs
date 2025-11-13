
using System;

[Serializable]
public class Model{

    public string id;

    public string name;

    public string base64Content;

    public long xoffset;

    public long yoffset;

    public long zoffset;

    
    override public string ToString(){
        return $"ID: {this.id}, Name: {this.name}, Xoffset: {this.xoffset}, Yoffset: {this.yoffset}, Zoffset: {this.zoffset}";
    }

}