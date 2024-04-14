﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;
using GHPC;
using GHPC.Utility;
using GHPC.Weapons;
using TMPro;
using UnityEngine;
using HarmonyLib;

namespace PactIncreasedLethality
{
    public class UVBU : MonoBehaviour
    {
        public FireControlSystem fcs;
        public GameObject readout_go;
        public TextMeshProUGUI readout;
        public float cd = 0f;

        void Update()
        {
            if (cd > 0f && readout.IsActive())
            {
                cd -= Time.deltaTime;
            }

            if (cd <= 0f && readout.IsActive())
            {
                cd = 0f;
                readout_go.SetActive(false);
            }

            /*
            bool button = InputUtil.MainPlayer.GetButtonDown("Lase");

            if (button)
            {
                cd = 2f;

                readout_go.SetActive(true);

                float flight_time = fcs._bc.GetFlightTime(fcs._bcAmmo, fcs._currentRange);       
                float x = fcs._averageTraverseRate.x * 0.017453292f * fcs._currentRange * flight_time * -10f;
                x /= (1f - fcs.transform.localPosition.x) * Mathf.Clamp(fcs._currentRange / 1500f, 0f, 1f);
                string sign = Math.Sign(x) > 0 ? "+" : "-";
                if ((int)x == 0) sign = "";

                int lead = Math.Abs(((int)MathUtil.RoundFloatToMultipleOf(x, 5)));

                if (lead > 999) lead = 999;

                readout.text = sign + lead.ToString("000");
            }
            */
        }
    }

    [HarmonyPatch(typeof(GHPC.Weapons.FireControlSystem), "DoLase")]
    public static class UVBULead
    {
        private static void Postfix(GHPC.Weapons.FireControlSystem __instance)
        {
            UVBU uvbu = __instance.GetComponentInChildren<UVBU>();

            if (uvbu == null) return;

            uvbu.cd = 2f;

            uvbu.readout_go.SetActive(true);

            float flight_time = __instance._bc.GetFlightTime(__instance._bcAmmo, __instance._currentRange);
            float x = __instance._averageTraverseRate.x * 0.017453292f * __instance._currentRange * flight_time * -10f;
            x /= (1f - __instance.transform.localPosition.x) * Mathf.Clamp(__instance._currentRange / 1500f, 0f, 1f);
            string sign = Math.Sign(x) > 0 ? "+" : "-";
            if ((int)x == 0) sign = "";

            int lead = (int)Math.Abs(MathUtil.RoundIntToMultipleOf((int)x, 5));

            if (lead > 999) lead = 999;

            uvbu.readout.text = sign + lead.ToString("000");
        }
    }
}
