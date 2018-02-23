using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour {
    public enum EffectId {        
            NEGATIVE = 0,
            POSITIVE = 1,
            INCREASE = 2
    }
    [SerializeField]    
    private int effectType,valueOfEffect,timeOfEffect;
    [SerializeField]
    private string effectDetail;
    
    public int GetEffectValue() {
        return this.valueOfEffect;
    }

    public string GetEffectDetail() {
        return this.effectDetail;
    }
    
	
}
