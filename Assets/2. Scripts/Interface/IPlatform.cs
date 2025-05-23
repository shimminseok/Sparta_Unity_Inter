using UnityEngine;

public interface IPlatform
{
    Transform PlatformTransform { get; }
    void      Execute(GameObject player);
    void      Exit(GameObject player);
}