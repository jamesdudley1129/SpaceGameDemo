using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipUI : MonoBehaviour
{
    //credits
    //said12 @twitch.tv sovlved the problem getting a objects world space and setting its screen space!!

    //move visual mouse functions to here from shipcontrols
    public ObjectStatus ActiveTarget;
    public Vector3 UI_eliment_Offset;
    public Vector3 VelocityScale;
    public GameObject GameBoundary;
    public float BoundaryWarningSensitivity;
    public GameObject AimBoundary;//scale is 2x AimScale
    public Text Inspector;
    public GameObject Nutrual_Raidial_marker;
    public GameObject Hostile_Raidial_marker;
    public GameObject Freindly_Raidial_marker;
    public GameObject ThrottleUI;
    public GameObject StatusUI;

    public GameObject Nutrual_Outline;
    public GameObject Freindly_Outline;
    public GameObject Hostile_Outline;

    public GameObject world_tracker;
    public GameObject Collision_Icon;
    public GameObject Colision_warning;
    public GameObject _3DVelocity_Indicator;
    public GameObject ProgradeRadialVelocity_marker;

    public GameObject Active_Nutrual_Target_Icon;
    public GameObject Active_Freindly_Target_Icon;
    public GameObject Active_Hostile_Target_Icon;

    public GameObject Prograde_Icon;
    public GameObject Retrograde_Icon;
    public float CollisionSensitivity = 1;
    GameObject _camera;
    GameObject player;
    public float AimScale;
    public Vector2 Fov;
    Vector2 center;
    public RectTransform this_cavas;
    Dictionary<GameObject, GameObject> objAndMarker = new Dictionary<GameObject, GameObject>();
    GameObject Collision_hazerd_icon_instance;
    GameObject Collision_warning_instance;
    GameObject GameBoundaryWarning_instance;
    GameObject GameBoundary_marker_instance;
    public GameObject Prograde_RadialVelocity_icon_instance;
    public GameObject FOVPro_Velocity_icon_instance;
    public GameObject FOVRetro_Velocity_icon_instance;
    public float OutlineMarkerRadius;
    public bool _3d_Vel_marker_active;
    public GameObject UI_Camrea;

    public float lastVelocityCheck = 0f;
    public void Awake()
    {

        AimBoundary.GetComponent<RectTransform>().sizeDelta = new Vector2(AimScale * 2, AimScale * 2);
        _camera = world_tracker.GetComponent<WorldTracker>().camrea;
    }

    GameObject DebugSphere;

    private void Update()
    {
        player = world_tracker.GetComponent<WorldTracker>().player;
        Fov = _camera.GetComponent<CameraBeh>().fov;
        if (player != null)
        {
            // BoundaryCheckUI
            UpdateBoundaryCheck();
            // CleanUpObjIconsCheck
            CleanUpObjIcons();
            // UpdateObjIconsUI
            UpdateObjectIcons();
            VelocityUI();
            ActiveTarget = player.GetComponent<ShipControls>().activeTarget;
        }

    }
    private void FixedUpdate()
    {
        if (player != null)
        {
            UpdateCollisionVector();
            SetThrottleValues(player.GetComponent<ShipControls>().localVelocity);
            SetStatusDisplay();
        }
    }
    enum VectorType
    {
        prograde, retrograde
    }
    void UpdateObjectIcons()
    {
        Inspector.text = "objects:";
        if (player.GetComponent<ShipSpecs>().RadarPlatform.transform != null)
        {
            foreach (ObjectStatus localObj in player.GetComponent<ShipSpecs>().RadarPlatform.transform.GetChild(0).GetComponent<RadarComponent>().localObjects)
            {
                if (localObj != null)
                {
                    Inspector.text += "/n *" + localObj.name + ",";
                    if (localObj.gameObject != world_tracker.GetComponent<WorldTracker>().player)
                    {
                        if (objAndMarker.ContainsKey(localObj.gameObject))
                        {
                            /*

                            Vector3 axis = _camrea.transform.forward;
                            Vector3 point = localObj.transform.position - _camrea.transform.position;
                            float angle = Vector3.Angle(point, axis);                       
                            Vector2 ObjectPointAngle = new Vector3(Mathf.Abs(local_dir.normalized.x * angle), Mathf.Abs(local_dir.normalized.y * angle));
                            */
                            Vector2 local_dir = _camera.transform.InverseTransformDirection(localObj.transform.position - _camera.transform.position);
                            Vector2 ObjectPointAngle = _camera.GetComponent<CameraBeh>().ObjectToScreenPos(localObj.transform.position);
                            objAndMarker.TryGetValue(localObj.gameObject, out GameObject icon);
                            float distance_from_player = Vector3.Distance(localObj.transform.position, player.transform.position);
                            //angle * dir.norm = point on circumference 
                            if (ObjectPointAngle.x <= Fov.x && ObjectPointAngle.y <= Fov.y)
                            {
                                Vector3 pos = new Vector3();
                                Vector3 scale = new Vector3();
                                SetScreenPos(localObj.gameObject.transform.position, out pos, out scale);
                                UpdateObjectOutline(icon, localObj.gameObject, pos, scale);//needs to be simplified
                                Set_Obj_Outline_Distance_Text(distance_from_player, icon);
                            }
                            else
                            {
                                UpdateRaidalMarker(icon, localObj, local_dir);
                            }

                        }
                        else
                        {
                            CreateUI_Object(localObj);//does alot of the same crap as UpdateObjectOutline this needs to be simplified
                        }
                    }
                }
            }
        }
    }//root for calling all things object --> icon/marker related 
    void CleanUpObjIcons()
    {
        List<GameObject> nolongerNeeded = new List<GameObject>();
        foreach (var item in objAndMarker)//removes objects that no longer exist or are no longer in radar range
        {
            if (item.Key == null)
            {
                nolongerNeeded.Add(item.Key);
            }
            else
            {
                if (!player.GetComponent<ShipSpecs>().RadarPlatform.transform.GetChild(0).GetComponent<RadarComponent>().localObjects.Contains(item.Key.GetComponent<ObjectStatus>()))
                {
                    nolongerNeeded.Add(item.Key);
                }
            }
        }
        foreach (GameObject remove in nolongerNeeded)
        {
            objAndMarker.TryGetValue(remove, out GameObject marker);
            Destroy(marker);
            objAndMarker.Remove(remove);
        }
    }
    void UpdateBoundaryCheck()
    {
        if (GameBoundary != null)
        {
            if (Vector3.Distance(Vector3.zero, player.transform.position) >= GameBoundary.transform.localScale.x - ((BoundaryWarningSensitivity / 100) * GameBoundary.transform.localScale.x))
            {
                if (GameBoundaryWarning_instance == null)
                {
                    GameBoundaryWarning_instance = Instantiate(Colision_warning, transform);
                }
                if (GameBoundary_marker_instance == null)
                {
                    GameBoundary_marker_instance = Instantiate(Collision_Icon, transform);
                }
                Vector2 dir = _camera.transform.InverseTransformDirection(-_camera.transform.position);
                Vector3 axis = _camera.transform.forward;
                Vector3 point = -_camera.transform.position;
                float angle = Vector3.Angle(point, axis);
                Vector2 ObjectPointAngle = new Vector3(Mathf.Abs(dir.normalized.x * angle), Mathf.Abs(dir.normalized.y * angle));

                float FOVangleV = _camera.GetComponent<Camera>().fieldOfView / 2;
                float FOVangleH = Camera.VerticalToHorizontalFieldOfView(_camera.GetComponent<Camera>().fieldOfView, _camera.GetComponent<Camera>().aspect) / 2;

                if (ObjectPointAngle.x <= FOVangleH && ObjectPointAngle.y <= FOVangleV)
                {

                    Vector3 pos = new Vector2(0, 0);
                    pos = _camera.GetComponent<Camera>().WorldToScreenPoint(pos);
                    pos.x -= this_cavas.rect.width / 2;
                    pos.y -= this_cavas.rect.height / 2;
                    pos.z = 0;
                    //var offset = _camrea.GetComponent<Camera>().WorldToScreenPoint(Vector3.zero);
                    GameBoundary_marker_instance.transform.localPosition = pos;
                }
                else
                {
                    GameBoundary_marker_instance.transform.localPosition = dir.normalized * (AimScale + 20);
                }

                float target_distance;
                // Abs(player.pos- target.pos)
                target_distance = Mathf.Abs(player.transform.position.magnitude - GameBoundary.transform.localScale.x);
                float relitive_speed_to_target;
                // Abs (pos change over time)
                if (GameBoundary.GetComponent<Rigidbody>())
                {
                    relitive_speed_to_target = (player.GetComponent<Rigidbody>().velocity + GameBoundary.GetComponent<Rigidbody>().velocity).magnitude;
                }
                else
                {
                    relitive_speed_to_target = player.GetComponent<Rigidbody>().velocity.magnitude;
                }


                //distance/velocity 
                float Collision_timer;
                // time = speed/distance
                Collision_timer = Mathf.RoundToInt(target_distance / relitive_speed_to_target);



                GameBoundaryWarning_instance.GetComponent<CollisionUIAlertBeh>().timer.GetComponent<Text>().text = "T- " + Collision_timer.ToString();

            }
            else
            {
                if (GameBoundaryWarning_instance != null)
                {
                    Destroy(GameBoundaryWarning_instance);
                    GameBoundaryWarning_instance = null;

                }
                if (GameBoundary_marker_instance != null)
                {
                    Destroy(GameBoundary_marker_instance);
                    GameBoundary_marker_instance = null;
                }
            }
        }
    }
    void UpdateCollisionVector()
    {
        Ray ray = new Ray(player.transform.position, player.GetComponent<Rigidbody>().velocity.normalized);
        RaycastHit[] hitsArray = Physics.RaycastAll(ray, Vector3.Distance(player.transform.position, player.transform.position + player.GetComponent<Rigidbody>().velocity) * CollisionSensitivity);

        GameObject Hazard = null;
        RaycastHit hitData = new RaycastHit();
        foreach (RaycastHit hit in hitsArray)
        {
            List<Transform> parents = new List<Transform>();
            Transform parent = null;
            if (hit.collider.transform != null)//see if compontnet is attached
            {
                parent = hit.collider.transform;
            }
            while (parent != null)
            {
                parents.Add(parent);
                if (parent.parent != null)
                {
                    parent = parent.parent;
                }
                else
                {
                    parent = null;
                }
            }
            if (!parents.Contains(player.transform))
            {
                Hazard = hit.collider.gameObject;
                hitData = hit;
                break;
            }

        }
        if (Hazard != null)
        {
            Debug.Log("Collison Found:" + Hazard.name);
            if (Collision_hazerd_icon_instance == null)
            {
                Collision_hazerd_icon_instance = Instantiate(Collision_Icon, transform);
            }
            if (Collision_warning_instance == null)
            {
                Collision_warning_instance = Instantiate(Colision_warning, transform);
            }
            Vector3 AvoidenceVector = -player.GetComponent<Rigidbody>().velocity + player.transform.position;//maybe i need to find the velocity relitive to the hazard
            Vector3 pos = _camera.GetComponent<Camera>().WorldToScreenPoint(AvoidenceVector);
            Vector2 dir = _camera.transform.InverseTransformDirection(AvoidenceVector - _camera.transform.position);
            Vector3 axis = _camera.transform.forward;
            Vector3 point = AvoidenceVector - _camera.transform.position;
            float angle = Vector3.Angle(point, axis);
            Vector2 ObjectPointAngle = new Vector3(Mathf.Abs(dir.normalized.x * angle), Mathf.Abs(dir.normalized.y * angle));
            float FOVangleV = _camera.GetComponent<Camera>().fieldOfView / 2;
            float FOVangleH = Camera.VerticalToHorizontalFieldOfView(_camera.GetComponent<Camera>().fieldOfView, _camera.GetComponent<Camera>().aspect) / 2;

            if (ObjectPointAngle.x <= FOVangleH && ObjectPointAngle.y <= FOVangleV)
            {


                pos.x -= this_cavas.rect.width / 2;
                pos.y -= this_cavas.rect.height / 2;
                pos.z = 0;
                Vector3 scale = new Vector3(OutlineMarkerRadius, OutlineMarkerRadius, 0);

                Collision_hazerd_icon_instance.transform.localPosition = pos;
                Collision_hazerd_icon_instance.transform.localScale = scale;
            }
            else
            {
                Collision_hazerd_icon_instance.transform.localPosition = dir.normalized * (AimScale + 20);
            }
            float target_distance;
            // Abs(player.pos- target.pos)
            target_distance = Vector3.Distance(player.transform.position, hitData.point);
            float relitive_speed_to_target;
            // Abs (pos change over time)

            //prob want to change
            if (hitData.collider.GetComponent<Rigidbody>())
            {

                //add x y and z from a and b
                //this will create relitive velocity
                relitive_speed_to_target = (player.GetComponent<Rigidbody>().velocity + hitData.collider.GetComponent<Rigidbody>().velocity).magnitude;
            }
            else
            {
                relitive_speed_to_target = player.GetComponent<Rigidbody>().velocity.magnitude;
            }

            //distance/velocity 
            Debug.Log("Collision Info: Distance = " + target_distance + " :: Relitive Speed = " + relitive_speed_to_target + " ||");
            float Collision_timer;
            // time = speed/distance
            Collision_timer = Mathf.RoundToInt(target_distance / relitive_speed_to_target);



            Collision_warning_instance.GetComponent<CollisionUIAlertBeh>().timer.GetComponent<Text>().text = "T- " + Collision_timer.ToString();
        }
        else
        {
            Destroy(Collision_warning_instance);
            Collision_warning_instance = null;

            Destroy(Collision_hazerd_icon_instance);
            Collision_hazerd_icon_instance = null;
        }



    }
    void VelocityUI()
    {
        Vector3 vel = player.GetComponent<ShipControls>().localVelocity;
        if (FOVPro_Velocity_icon_instance != null)
        {
            FOVPro_Velocity_icon_instance.GetComponent<VelocityDirIconbeh>().SetSpeedText(vel.magnitude);
        }
        if (FOVRetro_Velocity_icon_instance != null)
        {
            FOVRetro_Velocity_icon_instance.GetComponent<VelocityDirIconbeh>().SetSpeedText(-vel.magnitude);
        }
        if(vel.magnitude >= 0.05f)
        {
            if (!_3d_Vel_marker_active)//for the (UI)sprite based velocity system
            {
                _3DVelocity_Indicator.SetActive(false);
                SetProgrades(vel);
                SetRetrogrades(vel);
                
            }
            else
            {
                if (FOVPro_Velocity_icon_instance != null)//destroys since there is not velocity
                {
                    Destroy(FOVPro_Velocity_icon_instance);
                    FOVPro_Velocity_icon_instance = null;
                }
                if (FOVRetro_Velocity_icon_instance != null)//destroys since there is not velocity
                {
                    Destroy(FOVRetro_Velocity_icon_instance);
                    FOVRetro_Velocity_icon_instance = null;
                }
                if (Prograde_RadialVelocity_icon_instance != null)
                {
                    Destroy(Prograde_RadialVelocity_icon_instance);
                    Prograde_RadialVelocity_icon_instance = null;
                }
                _3DVelocity_Indicator.SetActive(true);
                Vector3 point = vel;
                _3DVelocity_Indicator.transform.rotation = Quaternion.LookRotation(point, _3DVelocity_Indicator.transform.up);
            }
        }
        else
        {

            if (Prograde_RadialVelocity_icon_instance != null)
            {
                Destroy(Prograde_RadialVelocity_icon_instance);
                Prograde_RadialVelocity_icon_instance = null;
            }
            if (FOVPro_Velocity_icon_instance != null)//destroys since there is not velocity
            {
                Destroy(FOVPro_Velocity_icon_instance);
                FOVPro_Velocity_icon_instance = null;
            }
            if (FOVRetro_Velocity_icon_instance != null)//destroys since there is not velocity
            {
                Destroy(FOVRetro_Velocity_icon_instance);
                FOVRetro_Velocity_icon_instance = null;
            }
            _3DVelocity_Indicator.SetActive(false);
        }
    }
    
    public void SetProgrades(Vector3 vel)
    {
        Vector3 ProgradeVector = _camera.transform.TransformPoint(vel);
        Vector2 local_dir = _camera.transform.InverseTransformDirection(ProgradeVector - _camera.transform.position);

        Vector2 ObjectPointAngle = _camera.GetComponent<CameraBeh>().ObjectToScreenPos(ProgradeVector);

        if (ObjectPointAngle.x <= Fov.x && ObjectPointAngle.y <= Fov.y && vel.z > 0.01f)
        {


            Vector3 pos = new Vector3();
            Vector3 scale = new Vector3();
            SetScreenPos(ProgradeVector, out pos, out scale);
            if (Prograde_RadialVelocity_icon_instance != null)//checks to see if raidalVelocity icon is still present
            {
                Destroy(Prograde_RadialVelocity_icon_instance);//destroys the current icon game object
                Prograde_RadialVelocity_icon_instance = null;
            }
            if (FOVPro_Velocity_icon_instance == null)
            {
                //creates icon
                FOVPro_Velocity_icon_instance = Instantiate(Prograde_Icon, transform);
            }
            //sets values for icon
            FOVPro_Velocity_icon_instance.transform.localPosition = pos;
            //FOV_Velocity_icon_instance.transform.localScale = scale;

        }
        else//create the radial directional icon instead
        {
            if (FOVPro_Velocity_icon_instance != null)
            {
                Destroy(FOVPro_Velocity_icon_instance);
                FOVPro_Velocity_icon_instance = null;
            }
            if (local_dir != Vector2.zero)
            {
                if (Prograde_RadialVelocity_icon_instance == null)
                {
                    //create

                    Prograde_RadialVelocity_icon_instance = Instantiate(ProgradeRadialVelocity_marker, transform);
                }
                Prograde_RadialVelocity_icon_instance.transform.localPosition = SetRaidialLocation(local_dir);

            }
        }

    }
    public void SetRetrogrades(Vector3 vel)
    {

        Vector3 RetrogradeVector = _camera.transform.TransformPoint(-vel);
        Vector2 local_dir = _camera.transform.InverseTransformDirection(RetrogradeVector - _camera.transform.position);
        Vector2 ObjectPointAngle = _camera.GetComponent<CameraBeh>().ObjectToScreenPos(RetrogradeVector);
        if (ObjectPointAngle.x <= Fov.x && ObjectPointAngle.y <= Fov.y && vel.z < -0.01)
        {

            Vector3 pos = new Vector3();
            Vector3 scale = new Vector3();
            SetScreenPos(RetrogradeVector, out pos, out scale);
            if (FOVRetro_Velocity_icon_instance == null)
            {
                //creates icon
                FOVRetro_Velocity_icon_instance = Instantiate(Retrograde_Icon, transform);
            }
            //sets values for icon
            FOVRetro_Velocity_icon_instance.transform.localPosition = pos;
            //FOV_Velocity_icon_instance.transform.localScale = scale;

        }
        else
        {
            if (FOVRetro_Velocity_icon_instance != null)
            {
                Destroy(FOVRetro_Velocity_icon_instance);
                FOVRetro_Velocity_icon_instance = null;
            }
        }
    }
    public void SetThrottleValues(Vector3 vel)
    {
        float acceliration = vel.magnitude;
        if (lastVelocityCheck != vel.magnitude)
        {
             acceliration = Mathf.Abs(vel.magnitude - lastVelocityCheck);
            lastVelocityCheck = vel.magnitude;
        }
        ThrottleUI.GetComponent<ThrottleUIEliment>().SetAcceleration(acceliration);
        ThrottleUI.GetComponent<ThrottleUIEliment>().SetThrottle(Mathf.RoundToInt(player.GetComponent<ShipControls>().throttle));
    }
    public void SetStatusDisplay()
    {
        ObjectStatus specs;
        if(player != null)
        {

           specs = player.GetComponent<ObjectStatus>();
           ShipStatusUIEliment UIeliment = StatusUI.GetComponent<ShipStatusUIEliment>();
           UIeliment.SetHull(specs.MaxHP, specs.HP);
           UIeliment.SetShield(specs.MaxShield, specs.Shield);
        }
    }
    void SetScreenPos(Vector3 point,out Vector3 pos,out Vector3 scale)
    {
        pos = new Vector2();

        pos = _camera.GetComponent<Camera>().WorldToScreenPoint(point);

        pos.x -= this_cavas.rect.width / 2;
        pos.y -= this_cavas.rect.height / 2;
        pos.z = 0;
        scale = new Vector3(OutlineMarkerRadius, OutlineMarkerRadius, 0);
    }
    void CreateUI_Object(ObjectStatus localObj)//create object marker if its not already in the dictonary (marker,object)
    {
        Vector2 dir = _camera.transform.InverseTransformDirection(localObj.transform.position - _camera.transform.position);
        Vector3 axis = _camera.transform.forward;
        Vector3 point = localObj.transform.position - _camera.transform.position;


        Vector2 xPoint = new Vector2(point.x, point.z);
        Vector2 yPoint = new Vector2(point.y, point.z);
        float angle = Vector3.Angle(point, axis);//might need later
        float angle_x = Vector2.Angle(new Vector2(axis.x, axis.z), xPoint);
        float angle_y = Vector2.Angle(new Vector2(axis.y, axis.z), yPoint);
        if (angle_x <= Fov.x && angle_y <= Fov.y)
        {
            Vector2 pos = new Vector2();
            pos.x = dir.normalized.x * angle_x;
            pos.y = dir.normalized.y * angle_y;
            GameObject Outline;
            if (localObj != ActiveTarget)
            {
                switch (localObj.RelationStatus)
                {
                    case Relation.Nutural:
                        Outline = Instantiate(Nutrual_Outline, transform);
                        break;
                    case Relation.Freindly:
                        Outline = Instantiate(Freindly_Outline, transform);
                        break;
                    case Relation.Hostile:
                        Outline = Instantiate(Hostile_Outline, transform);
                        break;
                    default:
                        Outline = null; 
                        break;
                }
            }
            else
            {
                switch (localObj.RelationStatus)
                {
                    case Relation.Nutural:
                        Outline = Instantiate(Active_Nutrual_Target_Icon, transform);
                        break;
                    case Relation.Freindly:
                        Outline = Instantiate(Active_Freindly_Target_Icon, transform);
                        break;
                    case Relation.Hostile:
                        Outline = Instantiate(Active_Hostile_Target_Icon, transform);
                        break;
                    default:
                        Outline = null;
                        break;
                }

            }
            Outline.transform.localPosition = pos;
            IconProperties properties = Outline.GetComponent<IconProperties>();
            properties.SetValues(localObj,world_tracker);
            objAndMarker.Add(localObj.gameObject, Outline);

        }
        else
        {
            GameObject go;
            switch (localObj.RelationStatus)
            {
                case Relation.Nutural:
                    go = Instantiate(Nutrual_Raidial_marker, transform);
                    break;
                case Relation.Freindly:
                    go = Instantiate(Freindly_Raidial_marker, transform);
                    break;
                case Relation.Hostile:
                    go = Instantiate(Hostile_Raidial_marker, transform);
                    break;
                default:
                    go = Instantiate(Nutrual_Raidial_marker, transform);
                    Debug.LogWarning("No relation found!");
                    break;
            }
            
            go.GetComponent<IconProperties>().SetValues(localObj,world_tracker);
            objAndMarker.Add(localObj.gameObject, go);
            Vector2 pos = dir.normalized * (AimScale + 20);
            // = Vector3.ClampMagnitude(dir,AimScale);
            //Vector3.Cross(dir.normalized, new Vector3(AimScale, AimScale, AimScale));
            go.transform.localPosition = pos;
        }
    }
    void UpdateObjectOutline(GameObject icon, GameObject localObj, Vector3 pos, Vector3 scale)//Updates Object Outline || updates existing ObjectOutlines Properties
    {
        if (ActiveTarget == localObj.GetComponent<ObjectStatus>() && ActiveTarget != null)
        {
            switch (localObj.GetComponent<ObjectStatus>().RelationStatus)
            {
                case Relation.Nutural:
                    if (!icon.name.StartsWith(Active_Nutrual_Target_Icon.name))//refrences the type to make sure its the same as its prefab
                    {
                        objAndMarker.Remove(localObj);
                        Destroy(icon);
                        GameObject Outline = Instantiate(Active_Nutrual_Target_Icon, transform);
                        Outline.transform.localPosition = pos;
                        Outline.transform.localScale = scale;
                        objAndMarker.Add(localObj, Outline);

                    }
                    else
                    {
                        icon.transform.localPosition = pos;
                        icon.transform.localScale = scale;
                    }

                    break;
                case Relation.Freindly:
                    if (!icon.name.StartsWith(Active_Freindly_Target_Icon.name))//refrences the type to make sure its the same as its prefab
                    {
                        objAndMarker.Remove(localObj);
                        Destroy(icon);
                        GameObject Outline = Instantiate(Active_Freindly_Target_Icon, transform);
                        Outline.transform.localPosition = pos;
                        Outline.transform.localScale = scale;
                        objAndMarker.Add(localObj, Outline);

                    }
                    else
                    {
                        icon.transform.localPosition = pos;
                        icon.transform.localScale = scale;
                    }
            
                    break;
                case Relation.Hostile:
                    if (!icon.name.StartsWith(Active_Hostile_Target_Icon.name))//refrences the type to make sure its the same as its prefab
                    {
                        objAndMarker.Remove(localObj);
                        Destroy(icon);
                        GameObject Outline = Instantiate(Active_Hostile_Target_Icon, transform);
                        Outline.transform.localPosition = pos;
                        Outline.transform.localScale = scale;
                        objAndMarker.Add(localObj, Outline);

                    }
                    else
                    {
                        icon.transform.localPosition = pos;
                        icon.transform.localScale = scale;
                    }

                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (localObj.GetComponent<ObjectStatus>().RelationStatus)
            {
                case Relation.Nutural:
                    if (!icon.name.StartsWith(Nutrual_Outline.name))//refrences the type to make sure its the same as its prefab
                    {
                        objAndMarker.Remove(localObj);
                        Destroy(icon);
                        GameObject Outline = Instantiate(Nutrual_Outline, transform);
                        Outline.transform.localPosition = pos;
                        Outline.transform.localScale = scale;
                        Outline.GetComponent<IconProperties>().SetValues(localObj.GetComponent<ObjectStatus>(), world_tracker);
                        objAndMarker.Add(localObj, Outline);
                    }
                    else
                    {
                        icon.transform.localPosition = pos;
                        icon.transform.localScale = scale;
                    }
                    break;
                case Relation.Freindly:
                    if (!icon.name.StartsWith(Freindly_Outline.name))//refrences the type to make sure its the same as its prefab
                    {
                        objAndMarker.Remove(localObj);
                        Destroy(icon);
                        GameObject Outline = Instantiate(Freindly_Outline, transform);
                        Outline.transform.localPosition = pos;
                        Outline.transform.localScale = scale;
                        Outline.GetComponent<IconProperties>().SetValues(localObj.GetComponent<ObjectStatus>(), world_tracker);
                        objAndMarker.Add(localObj, Outline);
                    }
                    else
                    {
                        icon.transform.localPosition = pos;
                        icon.transform.localScale = scale;
                    }
                    break;
                case Relation.Hostile:
                    if (!icon.name.StartsWith(Hostile_Outline.name))//refrences the type to make sure its the same as its prefab
                    {
                        objAndMarker.Remove(localObj);
                        Destroy(icon);
                        GameObject Outline = Instantiate(Hostile_Outline, transform);
                        Outline.transform.localPosition = pos;
                        Outline.transform.localScale = scale;
                        Outline.GetComponent<IconProperties>().SetValues(localObj.GetComponent<ObjectStatus>(), world_tracker);
                        objAndMarker.Add(localObj, Outline);
                    }
                    else
                    {
                        icon.transform.localPosition = pos;
                        icon.transform.localScale = scale;
                    }
                    break;
                default:
                    break;
            }

        }
    } 
    void UpdateRaidalMarker(GameObject icon,ObjectStatus localObj,Vector2 dir)//Updates To Raidal TypeMarker || Updates Existing Raidal Markers properties
    {
        GameObject Outline;
        switch (localObj.RelationStatus)
        {
            case Relation.Nutural:
                if (!icon.name.StartsWith(Nutrual_Raidial_marker.name))
                {
                    objAndMarker.Remove(localObj.gameObject);
                    Destroy(icon);
                    Outline = Instantiate(Nutrual_Raidial_marker, transform);
                    Outline.transform.localPosition = SetRaidialLocation(dir);
                    Outline.transform.localRotation = SetRaidialRotation(dir);
                    Outline.GetComponent<IconProperties>().SetValues(localObj, world_tracker);
                    objAndMarker.Add(localObj.gameObject, Outline);
                }
                else
                {
                    icon.transform.localPosition = SetRaidialLocation(dir);
                    icon.transform.localRotation = SetRaidialRotation(dir);
                }
                break;
            case Relation.Freindly:
                if (!icon.name.StartsWith(Freindly_Raidial_marker.name))
                {
                    objAndMarker.Remove(localObj.gameObject);
                    Destroy(icon);
                    Outline = Instantiate(Freindly_Raidial_marker, transform);
                    Outline.transform.localPosition = SetRaidialLocation(dir);
                    Outline.transform.localRotation = SetRaidialRotation(dir);
                    Outline.GetComponent<IconProperties>().SetValues(localObj, world_tracker);
                    objAndMarker.Add(localObj.gameObject, Outline);
                }
                else
                {
                    icon.transform.localPosition = SetRaidialLocation(dir);
                    icon.transform.localRotation = SetRaidialRotation(dir);
                }
                break;
            case Relation.Hostile:
                if (!icon.name.StartsWith(Hostile_Raidial_marker.name))
                {
                    objAndMarker.Remove(localObj.gameObject);
                    Destroy(icon);
                    Outline = Instantiate(Hostile_Raidial_marker, transform);
                    Outline.transform.localPosition = SetRaidialLocation(dir);
                    Outline.transform.localRotation = SetRaidialRotation(dir);
                    Outline.GetComponent<IconProperties>().SetValues(localObj, world_tracker);
                    objAndMarker.Add(localObj.gameObject, Outline);
                }
                else
                {
                    icon.transform.localPosition = SetRaidialLocation(dir);
                    icon.transform.localRotation = SetRaidialRotation(dir); ;
                }
                break;
            default:
                break;
        }

    }
    void Set_Obj_Outline_Distance_Text(float distance,GameObject icon)
    {
        //issue here doesnt seem serious

        if (distance >= 1000)
        {
            icon.GetComponent<TargetIcon>().Distance.text = Mathf.RoundToInt(distance / 1000).ToString() + "km";
        }
        else if (icon.GetComponent<TargetIcon>() != null)
        {
            icon.GetComponent<TargetIcon>().Distance.text = distance.ToString() + "km";
        }
    }
    Vector3 SetRaidialLocation(Vector2 localDir)
    {
        var pos = new Vector3();
        pos = localDir.normalized * (AimScale + 20);

        return pos;
    }
    Quaternion SetRaidialRotation(Vector2 localdir)
    {
        Vector3 rot = Quaternion.FromToRotation(Vector3.up,localdir).eulerAngles;
        return Quaternion.Euler(rot);
    }
}
