using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDescriptorProcessor  {

    void ProcessGameDescriptor(GameDescriptor gameDescriptor);

    void ProcessMultipleGameDescriptor(GameDescriptor[] gameDescriptors);

}
