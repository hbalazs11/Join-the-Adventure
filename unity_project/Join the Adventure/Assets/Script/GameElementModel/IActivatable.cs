using System;
using System.Collections;
using System.Collections.Generic;

public interface IActivatable  {
    
    bool IsActive();

    void SetActive(bool active);

    event EventHandler<EventArgs> OnActivationChange;
}
