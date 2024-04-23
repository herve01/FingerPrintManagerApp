create database if not exists finger_print_manager_db;
use finger_print_manager_db;

create table finger_print_manager_db_info(
	param varchar(100) not null,
    valeur mediumtext not null,
    primary key(param)
);
       
create table if not exists horaire_travail_semaine(
    id varchar(32),
	jour varchar(15) not null,
    est_ouvrable tinyint(1),
	heure_debut time null,
    heure_fin time null,
    numero int,
	created_at datetime,
    updated_at datetime,
    constraint pk_horaire_travail_semaine primary key(id)
);

create table if not exists date_exception(
    id varchar(32),
	description varchar(100) not null,
    date date,
	created_at datetime,
    updated_at datetime,
    constraint pk_date_exception primary key(id)
);

create table if not exists province(
	id int auto_increment,
	nom VARCHAR(30),
	created_at datetime,
	constraint pk_province PRIMARY KEY(id)
);

/*Ville ou territoire*/
create table if not exists zone(
	id INT auto_increment,
    type varchar(11),
	nom VARCHAR(50),
	province_id INT,
	created_at datetime,
	constraint pk_zone PRIMARY KEY(id),
	constraint fk_zone_province FOREIGN KEY(province_id) REFERENCES province(id) on update cascade
);

/*Commune, secteur et chefferie*/
create table if not exists commune(
	id INT auto_increment,
    type varchar(11),
	nom VARCHAR(50),
	zone_id INT,
	created_at datetime,
	constraint pk_commune PRIMARY KEY(id),
	constraint pk_commune_zone FOREIGN KEY(zone_id) REFERENCES zone(id) on update cascade
);

create table if not exists direction_provinciale(
	id varchar(32),
	province_id int not null,
	est_generale tinyint(1) default 0,
	created_at datetime,
    updated_at datetime,
	constraint pk_direction_provincial primary key(id),
	constraint fk_direction_province foreign key(province_id) references province(id) on update cascade
);

create table if not exists entite(
	id varchar(32),
	direction_id varchar(32) not null,
    type enum('Direction', 'Agence') default 'Agence',
	zone_id int not null,
	numero varchar(12) not null,
	avenue varchar(25) not null,
	commune_id int not null,
    est_principale tinyint(1) default 0,
    created_at datetime,
    updated_at datetime,
	constraint pk_entite primary key(id),
	constraint fk_entite_zone foreign key(zone_id) references zone(id),
	constraint fk_entite_direction foreign key(direction_id) references direction_provinciale(id) on update cascade,
    constraint fk_entite_commune foreign key(commune_id) references commune(id) on update cascade
);

create table if not exists user (
    id varchar(32),
    entite_id varchar(32) not null,
    nom varchar(30) NOT NULL,
    prenom varchar(30) not null,
    username varchar(30) not null,
	`type` enum('ADMIN', 'USER') not null,
    `sexe` enum('Homme','Femme') not null,
    `telephone` varchar(14) not null,
    email varchar(100) NOT NULL,
    passwd varchar(32) NOT NULL,
    m_salt varchar(32) NOT NULL,
    etat tinyint(1) default 1,
	created_at datetime,
    updated_at datetime,
    constraint pk_user PRIMARY KEY (id),
    constraint fk_user_entite foreign key(entite_id) references entite(id) on update cascade,
    constraint username_unique unique(username asc),
    constraint email_unique unique(email asc)
);

create table if not exists direction(
	id varchar(32),
	denomination varchar(200) not null,
	sigle varchar(10),
	mission text,
    est_generale tinyint(1) default 0,
	created_at datetime,
    updated_at datetime,
	constraint pk_direction primary key(id)
);

create table if not exists departement(
	id varchar(32),
	direction_id varchar(32),
	denomination varchar(200) not null,
	mission text,
	created_at datetime,
	updated_at datetime,
	constraint pk_departement primary key(id),
	constraint fk_departement_direction foreign key(direction_id) references direction(id) on update cascade
);

create table if not exists grade(
	id varchar(6),
	intitule varchar(50),
	type varchar(15), /* Agent, Cadre, Haut-cadre*/
	niveau int default 0,
	description text,
	created_at datetime,
    updated_at datetime,
	constraint pk_grade primary key(id)
);

create table if not exists fonction(
	id varchar(32),
	grade_id varchar(6) not null,
	intitule varchar(100) not null,
    niveau enum('Direction', 'Departement', 'Agence'),
    unite_id varchar(32) not null, /* Identifiant de la direction ou la departement ou le bureau d'affectation */
    entite_id varchar(32) not null,
	description text,
	created_at datetime,
    updated_at datetime,
	constraint pk_fonction primary key(id),
	constraint fk_fonction_grade foreign key(grade_id) references grade(id) on update cascade,
	constraint fk_fonction_entite foreign key(entite_id) references entite(id) on update cascade
);

create table if not exists niveau_etude(
	id varchar(32),
	intitule varchar(30) not null,
    niveau int not null,
    a_domaine tinyint(1) default 1,
    grade_recrutement_id varchar(6) not null,
	created_at datetime,
    updated_at datetime,
	constraint pk_niveau_etude primary key(id)
);

create table if not exists domaine_etude(
	id varchar(32),
	intitule varchar(300) not null,
	created_at datetime,
    updated_at datetime,
	constraint pk_domaine_etude primary key(id)
);

create table if not exists employe(
	id varchar(32),
	matricule varchar(14) unique,
	nom varchar(30) not null,
	post_nom varchar(30),
	prenom varchar(30),
	sexe enum('Homme', 'Femme') not null,
    photo mediumblob,
	etat_civil varchar(12) not null,
	lieu_naissance varchar(20) not null,
	date_naissance date not null,
	province_origine_id int default 0,
	personne_contact varchar(120),
    qualite_contact varchar(30),
    est_affecte tinyint(1) default 0,
	telephone VARCHAR(14),
	email VARCHAR(100) UNIQUE,
	numero varchar(12) not null,
	avenue varchar(25) not null,
	commune_id int not null,
    conjoint varchar(100),
    telephone_conjoint varchar(14),
	created_at datetime,
    updated_at datetime,
	constraint pk_employe primary key(id),
	constraint fk_employe_commune foreign key(commune_id) references commune(id) on update cascade,
    constraint fk_employe_province_origine foreign key(province_origine_id) references province(id) on update cascade
);

create table if not exists employe_etude(
	id varchar(32),
	employe_id varchar(32) not null,
	niveau_id varchar(32) not null,
    domaine_id varchar(32),
	annee_obtention int not null,
	created_at datetime,
    updated_at datetime,
    constraint pk_employe_niveau primary key(id),
    constraint fk_employe_niveau_employe foreign key(employe_id) references employe(id) on update cascade,
	constraint fk_employe_niveau_niveau_etude foreign key(niveau_id) references niveau_etude(id) on update cascade,
    constraint fk_employe_niveau_domaine_etude foreign key(domaine_id) references domaine_etude(id) on update cascade
);

create table if not exists enfant_employe(
	id varchar(32),
	employe_id varchar(32) not null,
	nom varchar(30) not null,
	post_nom varchar(30) not null,
	prenom varchar(30) not null,
	sexe enum('Homme', 'Femme') not null,
    date_naissance date not null,
    created_at datetime,
    updated_at datetime,
    constraint pk_enfant_employe primary key(id),
    constraint fk_enfant_employe_employe foreign key(employe_id) references employe(id) on update cascade
);

create table if not exists employe_empreinte(
	id varchar(32),
	employe_id varchar(32) not null,
    empreinte_image mediumblob not null,
	empreinte_template mediumblob not null,
    size int not null,
    finger enum('LL', 'LR', 'LM', 'LI', 'LT', 'RT', 'RI', 'RM', 'RR', 'RL'),
	created_at datetime,
    updated_at datetime,
	constraint pk_employe_empreinte primary key(id),
	constraint fk_employe_empreinte_employe foreign key(employe_id) references employe(id) on update cascade
);

create table if not exists periode(
	id varchar(10),
	mois int not null,
    annee int not null,
	created_at datetime,
    updated_at datetime,
	constraint pk_periode primary key(id)
);

create table if not exists presence(
	id varchar(32),
    periode_id varchar(10) not null,
	employe_id varchar(32) not null,
    date date,
    mode_pointage enum('Utilisateur', 'Empreinte','QrCode', 'Smart_card', 'RFID'),
    heure_arrivee time,
    heure_depart time,
	created_at datetime,
    updated_at datetime,
	constraint pk_presence primary key(id),
	constraint fk_presence_periode foreign key(periode_id) references periode(id) on update cascade,
	constraint fk_presence_employe foreign key(employe_id) references employe(id) on update cascade
);

create table if not exists employe_grade(
	id varchar(32),
	employe_id varchar(32) not null,
	grade_id varchar(6) not null,
    est_initial tinyint(1) default 0,
    type enum('Commissionnement', 'Officiel'),
	date date,
	created_at datetime,
    updated_at datetime,
	constraint pk_employe_grade primary key(id),
	constraint fk_employe_grade_employe foreign key(employe_id) references employe(id) on update cascade,
	constraint fk_employe_grade_grade foreign key(grade_id) references grade(id) on update cascade
);

create table if not exists employe_fonction(
	id varchar(32),
	employe_id varchar(32) not null,
	fonction_id varchar(32) not null,
	date date,
    type enum('Officiel', 'Interim'),
    state enum('Running', 'Pause') default 'Running',
    date_fin date,
	created_at datetime,
    updated_at datetime,
	constraint pk_employe_fonction primary key(id),
	constraint fk_employe_fonction_employe foreign key(employe_id) references employe(id) on update cascade,
	constraint fk_employe_fonction_fonction foreign key(fonction_id) references fonction(id) on update cascade
);

create table if not exists affectation(
	id varchar(32),
	employe_id varchar(32) not null,
	ancienne_entite_id varchar(32),
	nouvelle_entite_id varchar(32) not null,
    niveau enum('Direction', 'Departement', 'Agence'),
    unite_id varchar(32) not null, /* Identifiant de la direction ou la departement ou le bureau d'affectation */
	date date,
	created_at datetime,
    updated_at datetime,
	constraint pk_affectation primary key(id),
	constraint fk_affectation_employe foreign key(employe_id) references employe(id) on update cascade,
	constraint fk_affectation_ancienne_entite foreign key(ancienne_entite_id) references entite(id) on update cascade,
	constraint fk_affectation_nouvelle_entite foreign key(nouvelle_entite_id) references entite(id) on update cascade
);



delimiter %
drop PROCEDURE if exists sp_registre_presence_journaliere%
CREATE  PROCEDURE sp_registre_presence_journaliere(IN v_entite_id varchar(32), in v_date datetime)
BEGIN
    
	select e.nom, e.post_nom, e.prenom, e.sexe, e.matricule, addtime(v_date, p.heure_arrivee) heure_arrivee, 
    addtime(v_date, p.heure_depart) heure_depart, eg.grade_id grade, 
    ifnull(get_affectation_direction(a.unite_id, a.niveau), 'Non affectés') AS direction,
    get_entite_name(ifnull(get_employe_current_entite(e.id), '04fc711301f3c784d66955d98d399afb')) entite_name, 
    CASE 
        WHEN p.id is null THEN 0  
		ELSE 1
	END est_present
    from employe e
    inner join employe_grade eg
    on eg.employe_id = e.id and eg.id = get_employe_grade_actuel_id(e.id)
	inner join grade g ON g.id = eg.grade_id 
    left join affectation a ON a.id = get_employe_current_affectation(e.id) and a.nouvelle_entite_id = get_employe_current_entite(e.id)
    left JOIN entite et ON a.nouvelle_entite_id = et.id 
    left join 
    (
		select p.* 
		from presence p
        where date(p.date) = date(v_date)
	) p
    on p.employe_id = e.id
    where ((get_employe_current_entite(e.id) is null and v_entite_id = '04fc711301f3c784d66955d98d399afb') or get_employe_current_entite(e.id) = v_entite_id)
	order by g.niveau DESC, e.nom, e.post_nom, e.prenom asc;
END%


delimiter %
drop FUNCTION if exists get_employe_current_entite%
CREATE FUNCTION get_employe_current_entite(v_employe_id varchar(32)) 
RETURNS varchar(32)
begin
	declare v_entite_id varchar(32);
    
    select nouvelle_entite_id into v_entite_id
    from affectation 
    where employe_id = v_employe_id
    order by id desc
    limit 1;
    
    return v_entite_id;
end%

DELIMITER %
DROP FUNCTION IF EXISTS get_employe_grade_actuel_id%
CREATE  FUNCTION get_employe_grade_actuel_id(v_employe_id varchar(32)) 
RETURNS varchar(32)
BEGIN
	  DECLARE v_rep varchar(32);
  
      SELECT ed.id into v_rep 
      FROM employe_grade ed
      inner join grade g
      on ed.grade_id = g.id
      WHERE ed.employe_id = v_employe_id and ed.type = 'Officiel'
      order by g.niveau desc
      limit 1;
    
    return v_rep;
END %

delimiter %
drop function if exists get_employe_current_affectation%
create function get_employe_current_affectation(v_employe_id varchar(32))
returns varchar(32)
begin
	declare v_affectation_id varchar(32);
    
    select id into v_affectation_id
    from affectation 
    where employe_id = v_employe_id
    order by id desc
    limit 1;
    
    return v_affectation_id;
end
%



DELIMITER %
DROP FUNCTION IF EXISTS get_affectation_direction%
CREATE FUNCTION get_affectation_direction(v_unite_id varchar(32), v_type_unite varchar(9)) 
RETURNS varchar(200)
BEGIN 
	DECLARE v_rep varchar(200);
	DECLARE tempo INT;
    
	IF v_type_unite = 'Direction' THEN 
		SELECT denomination INTO v_rep
		FROM direction 
		WHERE id = v_unite_id;
	ELSE	
		IF v_type_unite = 'Departement' THEN 
			SELECT d.denomination INTO v_rep 
			FROM departement dv
				INNER JOIN direction d ON dv.direction_id = d.id 
			WHERE dv.id = v_unite_id;
		END IF;
	END IF;
	
	RETURN v_rep;

END %


DELIMITER %
DROP FUNCTION IF EXISTS get_entite_name%
CREATE  FUNCTION get_entite_name(v_entite_id varchar(32)) 
RETURNS varchar(100) 
BEGIN
	  DECLARE v_rep varchar(100);
  
      SELECT 
      CASE 
        	WHEN et.est_principale = 1 THEN "Siège social"  
            WHEN et.est_principale = 0 THEN CONCAT('Agence de ', z.nom) 
			ELSE null
      END INTO v_rep 
      FROM entite et 
      INNER JOIN zone z ON et.zone_id = z.id
      where et.id = v_entite_id;
    
    RETURN v_rep;
END %