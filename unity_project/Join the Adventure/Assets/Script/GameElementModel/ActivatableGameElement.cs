﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class ActivatableGameElement : GameElement, IActivatable {

    protected bool isActive;
    [field: NonSerialized]
    public event EventHandler<EventArgs> OnActivationChange;

    public ActivatableGameElement(string id) : base(id)
    {
    }

    public bool IsActive()
    {
        return isActive;
    }

    public virtual void SetActive(bool active)
    {
        this.isActive = active;
        if (OnActivationChange != null)
        {
            OnActivationChange(this, EventArgs.Empty);
        }
    }
}
