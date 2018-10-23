using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDescriptorProcessor  {

    void ProcessGameDescriptor(GameDescriptor gameDescriptor);

    void ProcessMultipleGameDescriptor(List<GameDescriptor> gameDescriptors);

    void ProcessImageResources(List<byte[]> images);

}
