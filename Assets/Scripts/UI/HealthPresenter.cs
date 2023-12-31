using Components;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI
{
    public class HealthPresenter : MonoBehaviour
    {
        [SerializeField] private HealthComponent _model;

        [SerializeField] private TextMeshProUGUI _healthValue;

        private void Start()
        {
            _model.HealthObservable.Subscribe(value => _healthValue.text = value.ToString()).AddTo(this);
        }
    }
}
