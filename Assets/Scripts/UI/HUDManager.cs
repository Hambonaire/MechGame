using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;

public class HUDManager : MonoBehaviour
{
    //[HideInInspector]
    public Volume globalVolume;
    UnityEngine.Rendering.Universal.ChromaticAberration chrAbb;
    UnityEngine.Rendering.Universal.DepthOfField depthOfField;

    [HideInInspector]
    public CinemachineVirtualCamera vCam;

    public GameObject waypointPrefab;
    public GameObject weaponDisplayPrefab;
    public GameObject emptyDisplayPrefab;

    public GameObject group0DisplayParent;
    public GameObject group1DisplayParent;
    public GameObject group2DisplayParent;

    [HideInInspector]
    public MechManager connectedPlayer;
    [HideInInspector]
    public WeaponSystem connectedSystem;

    [HideInInspector]
    public Objective currentObjective;
    public TextMeshProUGUI objectiveText;

    [HideInInspector]
    public List<Waypoint> waypoints = new List<Waypoint>();

    Image hudCrosshair;

    bool hitMark = false;
    public Image hitMarkerImage;

    // Zoom
    float camZoom = 60;

    // Shake
    float shakeTimer;

    // DoF
    float focalLength = 1;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        BuildPlayerHUD();
        globalVolume.profile.TryGet(out chrAbb);
        globalVolume.profile.TryGet(out depthOfField);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        UpdateHitMarker();

        UpdateObjectiveUI();

        UpdatePlayerHUD();

        UpdateCameraShake();

        UpdateVolumeEffects();

        UpdateCamZoom();

        UpdateCamDoF();
    }

    // Continuous Function Calls
    public void UpdateObjectiveUI()
    {
        if (currentObjective == null)
            return;

        objectiveText.text = "- " + currentObjective.objectiveText;

        switch (currentObjective.thisObjectiveType)
        {
            case ObjectiveType.Target:
                int deadTargets = 0;

                for (int i = 0; i < currentObjective.targetMechs.Count; i++)
                {
                    if (currentObjective.targetMechs[i].totallyDestroyed)
                        deadTargets++;
                }

                objectiveText.text += "   (" + deadTargets + "/" + currentObjective.targetMechs.Count + ")";

                break;
            case ObjectiveType.Travel:

                break;
        }
    }

    void UpdateHitMarker()
    {
        if (hitMark)
        {
            hitMarkerImage.color = new Color(1, 1, 1, 1) ;
        }
        else
        {
            float nextAlpha = hitMarkerImage.color.a - (3f * Time.deltaTime);

            if (nextAlpha < 0)
                nextAlpha = 0;

            hitMarkerImage.color = new Color(1, 1, 1, nextAlpha);
        }

        hitMark = false;
    }

    void UpdatePlayerHUD()
    {
        // Check active weapon group
        var group = connectedSystem.selectedGroup;

        group0DisplayParent.gameObject.SetActive(false);
        group1DisplayParent.gameObject.SetActive(false);
        group2DisplayParent.gameObject.SetActive(false);

        if (group == 0)
            group0DisplayParent.gameObject.SetActive(true);
        else if (group == 1)
            group1DisplayParent.gameObject.SetActive(true);
        else if (group == 2)
            group2DisplayParent.gameObject.SetActive(true);

        // Update weapon ammo counts
    }

    void UpdateCameraShake()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            var noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, 0, 1 - (shakeTimer/1));
        }
    }

    void UpdateVolumeEffects()
    {
        if (chrAbb.intensity.value > 0)
        {
            chrAbb.intensity.value -= Time.deltaTime * 1.5f;
        }
    }

    void UpdateCamZoom()
    {
        vCam.m_Lens.FieldOfView = camZoom;
    }

    void UpdateCamDoF()
    {
        depthOfField.focalLength.value = focalLength;
    }

    // One Time Function Calls
    public void InitObjective(Objective newObjective)
    {
        currentObjective = newObjective;

        foreach (Waypoint wp in waypoints)
            Destroy(wp.gameObject);

        waypoints.Clear();

        switch (currentObjective.thisObjectiveType)
        {
            case ObjectiveType.Target:

                for (int i = 0; i < currentObjective.targetMechs.Count; i++)
                {
                    var waypoint = Instantiate(waypointPrefab, this.transform).GetComponent<Waypoint>();
                    waypoints.Add(waypoint);
                    waypoint.Initialize(connectedPlayer.GetComponent<MechController>().mechCamera, currentObjective.targetMechs[i].transform);
                }

                break;
            case ObjectiveType.Travel:

                break;
        }
    }

    public void BuildPlayerHUD()
    {
        int g0cnt = 0;
        int g1cnt = 0;
        int g2cnt = 0;

        // Foreach weapon in mechManager weaponSys build weaponDisplay prefab
        for (int i = 0; i < connectedSystem.weaponExecutables.Count; i++)
        {
            var group = (int)connectedSystem.weaponExecutables[i].weaponItemRef.weaponClass;

            WeaponDisplay display = Instantiate(weaponDisplayPrefab, group0DisplayParent.transform).GetComponent<WeaponDisplay>();

            if (group == 0)
            {
                display.transform.parent = group0DisplayParent.transform;
                g0cnt++;
            }
            else if (group == 1)
            {
                display.transform.parent = group1DisplayParent.transform;
                g1cnt++;
            }
            else if (group == 2)
            {
                display.transform.parent = group2DisplayParent.transform;
                g2cnt++;
            }

            display.transform.localPosition = Vector3.zero;
            display.refExec = connectedSystem.weaponExecutables[i];
        }

        if (g0cnt == 0)
            Instantiate(emptyDisplayPrefab, group0DisplayParent.transform);
        if (g1cnt == 0)
            Instantiate(emptyDisplayPrefab, group1DisplayParent.transform);
        if (g2cnt == 0)
            Instantiate(emptyDisplayPrefab, group2DisplayParent.transform);

    }

    void CameraShake(float intensity)
    {
        var noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        noise.m_AmplitudeGain = intensity;
        shakeTimer = 1;
    }

    void ChromaticAbberation()
    {
        chrAbb.intensity.value = 0.75f;
    }

    public void OnZoom(float from, float to)
    {
        LeanTween.value(gameObject, from, to, 0.5f).setOnUpdate((float val) => { camZoom = val; }).setEaseOutExpo();

        LeanTween.value(gameObject, (from > to) ? 1: 100, (from > to) ? 100 : 1, 0.5f).setOnUpdate((float val) => { focalLength = val; }).setEaseOutExpo();
    }

    public void HitMarker()
    {
        hitMark = true;
    }

    public void OnFire(float intensity)
    {
        CameraShake(intensity / 15f);
    }

    public void GotHit(float damage)
    {
        CameraShake(damage / 10f);

        ChromaticAbberation();
    }
}
