using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class iconToggle : MonoBehaviour{
    
    public Sprite m_iconTrue;
    public Sprite m_iconFalse;

    public bool m_defaultIconState = true;

    Image m_image;

    void Start(){
        m_image = GetComponent<Image>();
        m_image.sprite = (m_defaultIconState) ? m_iconTrue:m_iconFalse;
    }

    public void ToggleIcon(bool state){
        Debug.Log(state);
        if(m_image && m_iconTrue && m_iconFalse){
            m_image.sprite = (state) ? m_iconTrue : m_iconFalse;
        }
    }
}
