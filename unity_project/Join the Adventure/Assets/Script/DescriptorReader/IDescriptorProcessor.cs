using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IDescriptorProcessor  {

    void ProcessGameDescriptor(GameDescriptor gameDescriptor);

    void ProcessMultipleGameDescriptor(List<GameDescriptor> gameDescriptors);

    void ProcessImageResources(Dictionary<string, MemoryStream> images);

}
