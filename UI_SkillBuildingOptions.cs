using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillBuildingOptions : MonoBehaviour // ��ų �ǹ��� �ɼ� UI�� ���� (���� ������� ���� ����)
{
    [SerializeField] public GameObject _elements = null;    // UI ��Ҹ� �����ϴ� ���� ������Ʈ
    [SerializeField] private Button _skillButton = null;    // ��ų ���� ��ư
    [SerializeField] private Button _upgradeButton = null;  // �ǹ� ���׷��̵� ��ư
    [SerializeField] private Button _infoButton = null;     // �ǹ� ���� ��ư
    [SerializeField] public Button _replaceButton = null;   // �ǹ� ���ġ ��ư
    private static UI_SkillBuildingOptions _instance = null; public static UI_SkillBuildingOptions instance { get { return _instance; } } // �̱��� �ν��Ͻ�

    public bool _isReplacing = false;   // ���ġ ����

    // ���� ������Ʈ�� ó�� ������ �� ȣ��Ǵ� �޼���
    private void Awake()
    {
        _instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
        _elements.SetActive(false); // UI ��Ȱ��ȭ
    }

    private void Start()
    {
        _skillButton.onClick.AddListener(skill);    // ��ų ��ư Ŭ�� �� skill �޼��� ȣ��
        _upgradeButton.onClick.AddListener(upgrade);    // �ǹ� ���׷��̵� ��ư Ŭ�� �� upgrade �޼��� ȣ��
        _infoButton.onClick.AddListener(info);  // �ǹ� ���� ��ư Ŭ�� �� info �޼��� ȣ��
        _replaceButton.onClick.AddListener(replace);    // �ǹ� ���ġ ��ư Ŭ�� �� replace �޼��� ȣ��
    }

    // UI Ȱ��ȭ ���¸� �����ϴ� �޼���
    public void SetStatus(bool status)
    {
        _elements.SetActive(status);
    }

    private void skill()
    {
        // UI�� skill�� ����� ��ų ���� UI�� ������ �Ѵ�.
    }

    // �ǹ� ���׷��̵� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void upgrade()
    {
        // UI�� upgrade�� ����� �ش� �ǹ��� ��ȭ UI�� ������ �Ѵ�.
    }

    // �ǹ� ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void info()
    {
        // UI�� info�� ����� �ش� �ǹ��� ������ ������ �Ѵ�.
    }

    // �ǹ� ���ġ ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void replace()
    {
        _isReplacing = !_isReplacing;   // ���ġ ���� ���
        if (!_isReplacing)
        {
            Building.selectedInstance._baseArea.gameObject.SetActive(false);    // ���ġ ���� �� ���ġ ���� ���� �Ǵ� �ٴ� ���� ��Ȱ��ȭ
        }
        else
        {
            Building.selectedInstance._baseArea.gameObject.SetActive(true);     // ���ġ ���� �� ���ġ ���� ���� �Ǵ� �ٴ� ���� Ȱ��ȭ
        }

        // ���� �������� : ���ġ ��ư�� ���� �� UI�� �����鼭 ó�� �ǹ��� �Ǽ����� ��ó�� Ȯ�� ��ư�� ������ �ϱ�

    }
}
