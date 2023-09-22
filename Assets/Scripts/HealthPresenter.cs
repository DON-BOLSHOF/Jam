using TMPro;
using UniRx;
using UnityEngine;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private Model _model;

    [SerializeField] private TextMeshProUGUI _healthValue;

    private void Start()
    {
        _model.HealthObservable.Subscribe(value => _healthValue.text = value.ToString()).AddTo(this);
    }
}
