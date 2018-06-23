using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using Random = System.Random;

namespace Services
{
    public class DocumentsService
    {
        private List<DocumentPreset> _documentPresets;
        private Random _random;
        
        public DocumentsService(List<DocumentPreset> presets)
        {
            _documentPresets = presets;
            _random = new Random();
        }

        public GameObject GenerateDocument()
        {
            var presetIndex = _random.Next(0, _documentPresets.Count);
            var preset = _documentPresets.ElementAt(presetIndex);

            return preset.Prefab;
        }
    }
}