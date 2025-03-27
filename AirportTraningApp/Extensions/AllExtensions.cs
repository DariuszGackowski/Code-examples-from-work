using CustomLocalisation;
using Module7;
using Module9;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static Module9.SecurityThreatsSO;
using static Module9.SecurityThreatsSO.Message;

namespace AllExtensions
{
    public static class AllExtensions
    {
        public static string GetInfoID(this List<Message> messages)
        {
            var foundMessage = messages.Find(message => message.IsCustom && message.InfoType != TypeInfo.None);

            if (foundMessage == null)
            {
                Debug.LogError("Nie znaleziono pasuj¹cej informacji o zagro¿eniu!");
                return "fire1";
            }

            return foundMessage.Identifier;
        }
        public static string GetPlaceID(this List<Message> messages)
        {
            var foundMessage = messages.Find(message => message.IsCustom && message.PlaceType != TypePlace.None);

            if (foundMessage == null)
            {
                Debug.LogError("Nie znaleziono pasuj¹cego miejsca!");
                return "approach3";
            }

            return foundMessage.Identifier;
        }
        public static void SetRandomAskList(this List<Message> messages)
        {
            if (!ThreatsSceneStarter.ErrorChance)
                return;

            System.Random random = new System.Random();
            var filteredMessages = messages.Where(message => message.TextType != TypeText.Entry && message.TextType != TypeText.None).ToList();
            if (filteredMessages.Count > 0)
            {
                var randomMessage = filteredMessages[random.Next(filteredMessages.Count)];
                randomMessage.IsToAsk = true;
            }
        }
        public static Message GetRandomElement(this List<Message> messages)
        {
            if (messages == null || messages.Count == 0) return null;
            return messages[UnityEngine.Random.Range(0, messages.Count)];
        }
        public static string GetRandomPlane(this List<ThreatScheme> schemes, TypeInfo infoType)
        {
            if (schemes == null || schemes.Count == 0) return null;

            var filteredSchemes = schemes.Where(scheme => scheme.InfoType == infoType).ToList();
            if (filteredSchemes.Count == 0) return null;

            List<string> planes = filteredSchemes.SelectMany(scheme => scheme.Plane).ToList();
            if (planes.Count == 0) return null;
            return planes[UnityEngine.Random.Range(0, planes.Count)];
        }
        public static ThreatScheme GetRandomSchemeByPlane(this List<ThreatScheme> schemes, TypeInfo infoType, string selectedPlane)
        {
            var matchingSchemes = schemes.Where(scheme => scheme.InfoType == infoType && scheme.Plane.Contains(selectedPlane)).ToList();
            if (matchingSchemes.Count == 0) return null;

            return matchingSchemes[UnityEngine.Random.Range(0, matchingSchemes.Count)];
        }
        public static Message GetRandomMessageById(this List<Message> messages, string id)
        {
            var filteredMessages = messages.Where(message => message.Identifier == id).ToList();
            return filteredMessages.GetRandomElement();
        }
        public static Message GetRandomMessageByInfoTypeAndId(this List<Message> messages, TypeInfo infoType, List<string> ids)
        {
            var filteredMessages = messages.Where(message => message.InfoType == infoType && ids.Contains(message.Identifier)).ToList();
            return filteredMessages.GetRandomElement();
        }

        public static Message GetRandomMessageByType(this List<Message> messages, TypePlane planeType)
        {
            var filteredMessages = messages.Where(message => message.PlaneType == planeType).ToList();
            return filteredMessages.GetRandomElement();
        }
        public static Message GetRandomMessageByApproaches(this List<Message> messages, List<string> approaches, TypeInfo infoType)
        {
            var filteredMessages = messages.Where(message =>
                (approaches.Contains(message.Identifier) && message.PlaceType == TypePlace.Approach) ||
                (message.PlaceType == TypePlace.Apron && (infoType == TypeInfo.InformationFire || infoType == TypeInfo.InformationMedical || infoType == TypeInfo.InformationSecurity))
            ).ToList();
            return filteredMessages.GetRandomElement();
        }
        public static Message GetRandomMessageById(this List<Message> messages, List<string> ids)
        {
            var filteredMessages = messages.Where(message => ids.Contains(message.Identifier)).ToList();
            return filteredMessages.GetRandomElement();
        }
        public static Message GetRandomInfo(this List<Message> messages)
        {
            List<Message> chosenMessages = messages.Where(message => message.InfoType == ThreatsSceneStarter.InfoType).ToList();
            return chosenMessages.GetRandomElement();
        }
        public static List<Message> GetRandomExcessList(this List<Message> messages, List<Message> excludedMessages)
        {
            System.Random random = new System.Random();
            List<Message> availableMessages = messages.Except(excludedMessages).Where(message => message.TextType != TypeText.Entry).ToList();

            if (availableMessages.Count == 0) return new List<Message>();

            return availableMessages.OrderBy(_ => random.Next()).Take(4).ToList();
        }
        public static string GetFullMessageText(this List<Message> messages)
        {
            return string.Join(" ", messages.Select(message => CustomLocalisationSettings.Singleton.GetTranslation(message.Identifier)));
        }
        public static void SetTransform(this Transform transform, Transform transformToSet)
        {
            transform.position = transformToSet.position;
            transform.rotation = transformToSet.rotation;
            transform.localScale = transformToSet.localScale;
        }
        public static void DeactivateObject(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
        public static void DeactivateObjectRenderer(this GameObject gameObject)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        public static void DeactivateObjectCollider(this GameObject gameObject)
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }
        public static void ActivateObject(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
        public static void ActivateHighlight(this GameObject gameObject)
        {
            List<Material> materials = gameObject.GetComponent<MeshRenderer>().materials.ToList();
            foreach (Material material in materials)
            {
                if (!material.HasProperty("_HighlightColor")) continue;

                Color highlightColor = material.GetColor("_HighlightColor");

                highlightColor.a = 1f;
                material.SetColor("_HighlightColor", highlightColor);
            }
        }
        public static void DeactivateHighlight(this GameObject gameObject)
        {
            List<Material> materials = gameObject.GetComponent<MeshRenderer>().materials.ToList();
            foreach (Material material in materials)
            {
                if (!material.HasProperty("_HighlightColor")) continue;

                Color highlightColor = material.GetColor("_HighlightColor");

                highlightColor.a = 0f;
                material.SetColor("_HighlightColor", highlightColor);
            }
        }
        public static void ActivateObjectRenderer(this GameObject gameObject)
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        public static void ActivateObjectCollider(this GameObject gameObject)
        {
            gameObject.GetComponent<Collider>().enabled = true;
        }
        public static void SetMaterial(this MeshRenderer meshRenderer, Material material)
        {
            meshRenderer.material = material;
        }
        public static void AddCollider(this XRSimpleInteractable xrSimpleInteractable, Collider collider)
        {
            if (xrSimpleInteractable.colliders.Contains(collider))
            {
                Debug.LogWarning($"{xrSimpleInteractable.name} contains collider from {collider.name}", collider.gameObject);
                return;
            }
            xrSimpleInteractable.colliders.Add(collider);
        }
        public static void AddHoverObject(this DamageSegment damageSegment, GameObject HoveredObject)
        {
            if (damageSegment.HoveredObjects.Contains(HoveredObject))
            {
                Debug.LogWarning($"{damageSegment.name} contains hover game object from {HoveredObject.name}", HoveredObject);
                return;
            }
            damageSegment.HoveredObjects.Add(HoveredObject);
        }
        public static void AddHoveredDisplacedPair(this DamageSegment damageSegment, HoveredDisplacedPair hoveredDisplacedPair)
        {
            if (damageSegment.HoveredDisplacedPairObjects.Contains(hoveredDisplacedPair))
            {
                Debug.LogWarning($"{damageSegment.name} contains displaced hover game objects from {hoveredDisplacedPair.SourceObject.name}", hoveredDisplacedPair.SourceObject);
                return;
            }
            damageSegment.HoveredDisplacedPairObjects.Add(hoveredDisplacedPair);
        }
        public static void AddDisplacedDamage(this DamageSegment damageSegment, DisplacedDamage displacedDamage)
        {
            if (damageSegment.DisplacedDamages.Contains(displacedDamage))
            {
                Debug.LogWarning($"{damageSegment.name} contains hover game object from {displacedDamage.gameObject.name}", displacedDamage.gameObject);
                return;
            }
            damageSegment.DisplacedDamages.Add(displacedDamage);
        }
        public static void AddAdditionalDamage(this DamageSegment damageSegment, AdditionalDamage additionalDamage)
        {
            //if (damageSegment.AdditionalDamages.Contains(additionalDamage))
            //{
            //    Debug.LogWarning($"{damageSegment.name} contains hover game object from {additionalDamage.gameObject.name}", additionalDamage.gameObject);
            //    return;
            //}
            //damageSegment.AdditionalDamages.Add(additionalDamage);
        }
        #region BARYCENTRIC COORDINATES
        /// <summary>
        /// Zwraca koordynaty UV na p³aszczyŸnie, wzglêdem punktów referencyjnych
        /// </summary>
        /// <param name="P">Position</param>
        /// <param name="A">Reference point</param>
        /// <param name="B">Reference point</param>
        /// <param name="C">Reference point</param>
        /// <returns>UV vector</returns>
        public static Vector2 XYZtoUV(this Vector3 P, Vector3 A, Vector3 B, Vector3 C)
        {
            var AB = B - A;
            var AC = C - A;
            var AP = P - A;
            var area = Vector3.Cross(AB, AC);
            var area2 = 1.0f / Vector3.Dot(area, area);
            var u = Vector3.Dot(area, Vector3.Cross(AP, AC)) * area2;
            var v = Vector3.Dot(area, Vector3.Cross(AB, AP)) * area2;
            return new Vector2(u, v);
        }
        //https://en.wikipedia.org/wiki/Line%E2%80%93plane_intersection
        /// <summary>
        /// Zwraca koordynaty UVW w przestrzeni 3D, wzglêdem punktów referencyjnych
        /// </summary>
        /// <param name="P">Position</param>
        /// <param name="A">Reference point</param>
        /// <param name="B">Reference point</param>
        /// <param name="C">Reference point</param>
        /// <param name="D">Reference point</param>
        /// <returns>UVW vector</returns>
        public static Vector3 XYZtoUVW(this Vector3 P, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
        {
            Vector3 Pab = B - A;
            Vector3 Pac = C - A;
            Vector3 Pad = A - D;
            Vector3 r = P - A;
            Vector3 area = Vector3.Cross(Pab, Pac);
            float det = 1.0f / -Vector3.Dot(Pad, area);
            float t = Vector3.Dot(area, r) * det;
            float u = Vector3.Dot(Vector3.Cross(Pad, Pac), r) * det;
            float v = Vector3.Dot(Vector3.Cross(Pab, Pad), r) * det;
            return new Vector3(u, v, t);
        }

        /// <summary>
        /// Odtwarza pozycjê 3D wzglêdem punktów ABC na p³aszczyŸnie tych puktów.
        /// </summary>
        /// <param name="UV">Barycentric coordinates 2D</param>
        /// <param name="A">Reference point</param>
        /// <param name="B">Reference point</param>
        /// <param name="C">Reference point</param>
        /// <returns>Decoded position</returns>
        public static Vector3 UVtoXYZ(this Vector2 UV, Vector3 A, Vector3 B, Vector3 C)
        {
            return A + (B - A) * UV.x + (C - A) * UV.y;
        }
        /// <summary>
        /// Odtwarza pozycjê 3D wzglêdem punktów ABCD
        /// </summary>
        /// <param name="UVW">Barycentric coordinates 3D</param>
        /// <param name="A">Reference point</param>
        /// <param name="B">Reference point</param>
        /// <param name="C">Reference point</param>
        /// <param name="D">Reference point</param>
        /// <returns>Decoded position</returns>
        public static Vector3 UVWtoXYZ(this Vector3 UVW, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
        {
            return A + (B - A) * UVW.x + (C - A) * UVW.y + (D - A) * UVW.z;
        }
        #endregion
    }
}