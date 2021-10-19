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
            StopCoroutine("Swing");// 코루틴 중단 함수 같은 코루틴을 시작하기위해서 동작하고 있는 코루틴을
                                   // 중단할 필요가 있음으로 StopCoroutine을 먼저실행
            StartCoroutine("Swing");// 코루틴 시작함수
        }
        else if (type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }
    IEnumerator Swing()
    {
        // yield break; 맨 윗줄에 쓸 경우 아래에 있는 yield return은 전부 비활성화된다.
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

        // yield break로 탈출가능

        // yield return null; // null은 1프레임 대기
        // yield return은 여러개 작성가능 yield 키워드로 시간차 로직 작성 가능

    }
    // 일반 함수 : {Use() 메인루틴 -> Swing 서브루틴 -> Use() 메인루틴 }(교차실행)
    // 코루틴 함수 : {Use() 메인루틴 + Swing() 코루틴(Co-op)
    IEnumerator Shot()
    {
        // 1.총알 발사
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null;

        // 2.탄피배출
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }

}
