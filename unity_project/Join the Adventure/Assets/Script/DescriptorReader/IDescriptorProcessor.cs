using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IDescriptorProcessor  {

    void ProcessMultipleGameDescriptor(List<GameDescriptor> gameDescriptors, Dictionary<string, MemoryStream> images);

}
