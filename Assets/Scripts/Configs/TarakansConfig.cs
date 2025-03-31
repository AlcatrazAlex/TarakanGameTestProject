using System.Linq;
using TaranaGame.ConfigsConfigs;
using TaranaGame.Tarakan;
using UnityEngine;

namespace TaranaGame.Configs
{
    [CreateAssetMenu(fileName = "TarakansConfig", menuName = "Configs/TarakansConfig")]
    public class TarakansConfig : ScriptableObject
    {
        [SerializeField] private TarakanController _tarakanController;
        [SerializeField] private TarakanViewData[] _tarakansConfigs;
        [SerializeField] private string[] _startTarakansKeys;
        
        public TarakanController TarakanControllerPrefab => _tarakanController;
        public string[] StartTarakansKeys => _startTarakansKeys;

        public TarakanViewBase GetTarakanConfig(string tarakanId)
        {
            return _tarakansConfigs.FirstOrDefault(t => t.TarakanId == tarakanId)?.TarakanView;
        }
    }
}