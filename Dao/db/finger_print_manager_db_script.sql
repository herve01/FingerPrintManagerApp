create database if not exists finger_print_manager_db;
use finger_print_manager_db;

create table finger_print_manager_db_info(
	param varchar(100) not null,
    valeur mediumtext not null,
    primary key(param)
);
       
create table if not exists horaire_travail_semaine(
    id int,
	designation varchar(10) not null,
    est_ouvrable tinyint(1),
	heure_debut time null,
    heure_fin time null,
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
    constraint fk_entite_commune foreign key(commune_id) references commune(id) on update cascade,
    constraint unique_entite_zone unique(zone_id)
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
    est_affecte tinyint(1) default 0,
	telephone VARCHAR(14),
	email VARCHAR(100) UNIQUE,
	created_at datetime,
    updated_at datetime,
	constraint pk_employe primary key(id)
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
	id varchar(32),
	mois int not null,
    annee int not null,
	created_at datetime,
    updated_at datetime,
	constraint pk_periode primary key(id)
);

create table if not exists presence(
	id varchar(32),
    periode_id varchar(32) not null,
	employe_id varchar(32) not null,
    date date,
    mode enum('Utilisateur', 'Empreinte','QrCode', 'Smart_card', 'RFID'),
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

create table if not exists affectation(
	id varchar(32),
	employe_id varchar(32) not null,
	ancienne_entite_id varchar(32),
	nouvelle_entite_id varchar(32) not null,
    niveau enum('Direction', 'Departement'),
    unite_id varchar(12) not null, /* Identifiant de la direction ou la division ou le bureau d'affectation */
	date date,
	created_at datetime,
    updated_at datetime,
	constraint pk_affectation primary key(id),
	constraint fk_affectation_employe foreign key(employe_id) references employe(id) on update cascade,
	constraint fk_affectation_ancienne_entite foreign key(ancienne_entite_id) references entite(id) on update cascade,
	constraint fk_affectation_nouvelle_entite foreign key(nouvelle_entite_id) references entite(id) on update cascade
);