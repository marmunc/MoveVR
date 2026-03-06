using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Project.Scripts
{
    [RequireComponent(typeof(XRSimpleInteractable))]
    public sealed class VrButtonActivationLabel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;

        private XRSimpleInteractable _interactable;
        private Coroutine _pendingRoutine;

        private void Awake()
        {
            _interactable = GetComponent<XRSimpleInteractable>();
            _interactable.selectEntered.AddListener(OnSelectEntered);

            if (_label != null)
                _label.text = "Press me (Hand or Ray)";
        }

        private void OnDestroy()
        {
            _interactable.selectEntered.RemoveListener(OnSelectEntered);

            if (_pendingRoutine != null)
                StopCoroutine(_pendingRoutine);
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (_pendingRoutine != null)
                StopCoroutine(_pendingRoutine);

            _pendingRoutine = StartCoroutine(HandleNearFarAndRelease(args.interactorObject));
        }

        private IEnumerator HandleNearFarAndRelease(IXRSelectInteractor originalInteractor)
        {
            yield return null;

            if (originalInteractor is XRInteractionGroup group &&
                group.activeInteractor is IXRSelectInteractor active)
            {
                originalInteractor = active;
            }

            if (originalInteractor is NearFarInteractor nearFar)
            {
                var region = nearFar.selectionRegion.Value;

                SetText(region switch
                {
                    NearFarInteractor.Region.Far => "Ray",
                    NearFarInteractor.Region.Near => "Hand",
                    _ => "Unknown"
                });
            }
            else if (originalInteractor is XRPokeInteractor)
            {
                SetText("Hand (Poke)");
            }
            else
            {
                SetText("Unknown");
            }

            var manager = _interactable.interactionManager;

            if (manager != null &&
                _interactable.isSelected &&
                originalInteractor is XRBaseInteractor baseInteractor &&
                baseInteractor.interactablesSelected.Contains(_interactable))
            {
                manager.SelectExit(originalInteractor, _interactable);
            }

            _pendingRoutine = null;
        }

        private void SetText(string how)
        {
            if (_label != null)
                _label.text = $"Activated by: {how}";
        }
    }
}