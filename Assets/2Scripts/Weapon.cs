using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range};
    public Type type;
    public int damege;
    public float rate;
    public int maxAmmo;
    public int curAmmo;

    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");// �ڷ�ƾ �ߴ� �Լ� ���� �ڷ�ƾ�� �����ϱ����ؼ� �����ϰ� �ִ� �ڷ�ƾ��
                                   // �ߴ��� �ʿ䰡 �������� StopCoroutine�� ��������
            StartCoroutine("Swing");// �ڷ�ƾ �����Լ�
        }
        else if (type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }
    IEnumerator Swing()
    {
        // yield break; �� ���ٿ� �� ��� �Ʒ��� �ִ� yield return�� ���� ��Ȱ��ȭ�ȴ�.
        // 1
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        // 2
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        // 3
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;

        // yield break�� Ż�Ⱑ��

        // yield return null; // null�� 1������ ���
        // yield return�� ������ �ۼ����� yield Ű����� �ð��� ���� �ۼ� ����

    }
    // �Ϲ� �Լ� : {Use() ���η�ƾ -> Swing �����ƾ -> Use() ���η�ƾ }(��������)
    // �ڷ�ƾ �Լ� : {Use() ���η�ƾ + Swing() �ڷ�ƾ(Co-op)
    IEnumerator Shot()
    {
        // 1.�Ѿ� �߻�
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null;

        // 2.ź�ǹ���
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }

}
