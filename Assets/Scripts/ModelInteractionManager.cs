using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class ModelInteractionManager : MonoBehaviour
{

    private static ModelInteractionManager instance;

    [SerializeField]
    GameObject prefab;

    public static ModelInteractionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<ModelInteractionManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("ModelInteractionManager");
                    instance = obj.AddComponent<ModelInteractionManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void MakeParent(GameObject container, GameObject child) {
        Transform parentTransform = container.transform;
        Transform childTransform = child.transform;
        childTransform.SetParent(parentTransform);
        child.layer = container.layer;
        childTransform.localPosition = Vector3.zero;
        childTransform.localRotation = Quaternion.identity;
        childTransform.localScale = Vector3.one;
    }

    public void Spawn(GameObject container, Interaction interactions) {
        GameObject interactableWrapper = Instantiate(prefab);
        interactableWrapper.transform.position = container.transform.position;
        GameObject wrapper = interactableWrapper.transform.GetChild(0).gameObject;
        GrabFreeTransformer transformer = wrapper.GetComponent<GrabFreeTransformer>();
        this.MakeParent(wrapper, container);
        this.CountCollidersInModel(container);
        this.FitColliderToBounds(wrapper);
        //this.AddCollidersToModel(wrapper);
        this.SetInteractions(transformer, interactions);
    }

    private void SetInteractions(GrabFreeTransformer transformer, Interaction interaction) {
        if(transformer == null || interaction == null){
            return;
        }
        TransformerUtils.RotationConstraints rotationConstraints = new TransformerUtils.RotationConstraints()
        {
            XAxis = new TransformerUtils.ConstrainedAxis(){
                ConstrainAxis = !interaction.rotateX
            },
            YAxis = new TransformerUtils.ConstrainedAxis(){
                ConstrainAxis = !interaction.rotateY
            },
            ZAxis = new TransformerUtils.ConstrainedAxis(){
                ConstrainAxis = !interaction.rotateZ
            }
        };
        TransformerUtils.PositionConstraints positionConstraints = new TransformerUtils.PositionConstraints()
        {
            XAxis = new TransformerUtils.ConstrainedAxis(){
                ConstrainAxis = !interaction.transformX
            },
            YAxis = new TransformerUtils.ConstrainedAxis(){
                ConstrainAxis = !interaction.transformY
            },
            ZAxis = new TransformerUtils.ConstrainedAxis(){
                ConstrainAxis = !interaction.transformZ
            }
        };
        transformer.InjectOptionalPositionConstraints(positionConstraints);
        transformer.InjectOptionalRotationConstraints(rotationConstraints);
    }

    private void CountCollidersInModel(GameObject model) {
        Collider[] colliders = model.GetComponentsInChildren<Collider>();
        if(GetComponent<Collider>() != null){
            Debug.LogWarning($"Find {colliders.Length} in model");
        } else {
            Debug.LogWarning($"No colliders found");
        }
    }

    private void AddCollidersToModel(GameObject wrappedModel) {
        Renderer[] renderers = wrappedModel.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers){
            if (renderer is MeshRenderer) {
                
                MeshFilter meshFilter = renderer.gameObject.GetComponent<MeshFilter>();
                Mesh mesh = meshFilter.mesh;
                renderer.gameObject.AddComponent<MeshCollider>();
                MeshCollider collider = renderer.gameObject.GetComponent<MeshCollider>();
                collider.sharedMesh = mesh;
                collider.convex = true;
                collider.isTrigger = true;

            } else {
                GameObject gobj = renderer.gameObject;
                BoxCollider collider = gobj.GetComponent<BoxCollider>(); 
                Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
                if (collider == null) {
                    gobj.AddComponent<BoxCollider>();
                    collider = gobj.GetComponent<BoxCollider>();
                }
                bounds.Encapsulate(renderer.bounds);
                collider.isTrigger = true;
                collider.center = gobj.transform.InverseTransformPoint(bounds.center);
                collider.size = bounds.size;
            }
        }
    }


    private void FitColliderToBounds(GameObject wrapperObject) {
        // Find the BoxCollider on the wrapper object
        BoxCollider boxCollider = wrapperObject.GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            wrapperObject.AddComponent<BoxCollider>();
            boxCollider = wrapperObject.GetComponent<BoxCollider>();
        }

        boxCollider.isTrigger = true;

        // This is the core logic: Calculate the collective bounds of all MeshRenderers in the children.
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        
        // Get all renderers in children, ignoring the parent wrapper itself.
        Renderer[] renderers = wrapperObject.GetComponentsInChildren<Renderer>();

        bool first = true;
        foreach (Renderer renderer in renderers)
        {
             if (first)
                {
                    // Initialize the bounds with the first renderer's bounds
                    bounds = renderer.bounds;
                    first = false;
                }
             // Encapsulate all subsequent renderers into the growing bounds
            bounds.Encapsulate(renderer.bounds);
        }
        
        if (first)
        {
            Debug.LogWarning("No MeshRenderers found in children to calculate bounds.");
            return;
        }

        // 1. Move the BoxCollider's Center to the center of the calculated bounds.
        // The bounds are in world space, so we convert the center to local space relative to the wrapper.
        boxCollider.center = wrapperObject.transform.InverseTransformPoint(bounds.center);
        
        // 2. Set the BoxCollider's Size to the calculated bounds size.
        boxCollider.size = bounds.size;

        Debug.Log($"BoxCollider fitted with center: {boxCollider.center} and size: {boxCollider.size}");
    }

}
